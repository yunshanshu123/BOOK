using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BookCategoryService
{
    private readonly BookCategoryTreeOperation _repository;
    private readonly LogService _logService;

    public BookCategoryService(BookCategoryTreeOperation repository, LogService logService)
    {
        _repository = repository;
        _logService = logService;
    }

    // 获取分类树
    public async Task<List<CategoryNode>> GetCategoryTreeAsync()
    {
        var allCategories = await _repository.GetAllCategoriesAsync();
        var categoryList = allCategories.ToList();
        
        // 构建树形结构
        var rootNodes = new List<CategoryNode>();
        var nodeDict = new Dictionary<string, CategoryNode>();

        // 创建所有节点
        foreach (var category in categoryList)
        {
            var node = new CategoryNode
            {
                CategoryID = category.CategoryID,
                CategoryName = category.CategoryName,
                ParentCategoryID = category.ParentCategoryID
            };
            nodeDict[category.CategoryID] = node;
        }

        // 构建父子关系
        foreach (var node in nodeDict.Values)
        {
            if (string.IsNullOrEmpty(node.ParentCategoryID))
            {
                rootNodes.Add(node);
            }
            else if (nodeDict.ContainsKey(node.ParentCategoryID))
            {
                nodeDict[node.ParentCategoryID].Children.Add(node);
            }
        }

        return rootNodes;
    }

    // 添加分类
    public async Task<bool> AddCategoryAsync(Category category, string operatorId)
    {
        try
        {
            // 检查分类名称是否重复
            if (await _repository.IsCategoryNameDuplicateAsync(category.CategoryName, category.ParentCategoryID))
            {
                throw new Exception("同级分类中已存在相同名称");
            }

            var result = await _repository.AddCategoryAsync(category);
            
            // 记录操作日志
            await _logService.LogCategoryAddAsync(category.CategoryName, category.CategoryName, operatorId);
            
            return result > 0;
        }
        catch (Exception ex)
        {
            await _logService.LogOperationFailureAsync("添加分类", "Librarian", operatorId, ex.Message);
            throw;
        }
    }

    // 更新分类
    public async Task<bool> UpdateCategoryAsync(Category category, string operatorId)
    {
        try
        {
            // 检查分类是否存在
            var existingCategory = await _repository.GetCategoryByIdAsync(category.CategoryID);
            if (existingCategory == null)
            {
                throw new Exception("分类不存在");
            }

            // 检查分类名称是否重复（排除当前分类）
            if (await _repository.IsCategoryNameDuplicateAsync(category.CategoryName, category.ParentCategoryID, category.CategoryID))
            {
                throw new Exception("同级分类中已存在相同名称");
            }

            var result = await _repository.UpdateCategoryAsync(category);
            
            // 记录操作日志
            await _logService.LogCategoryUpdateAsync(existingCategory.CategoryName, category.CategoryName, category.CategoryName, operatorId);
            
            return result > 0;
        }
        catch (Exception ex)
        {
            await _logService.LogOperationFailureAsync("更新分类", "Librarian", operatorId, ex.Message);
            throw;
        }
    }

    // 删除分类
    public async Task<bool> DeleteCategoryAsync(string categoryId, string operatorId)
    {
        try
        {
            // 检查分类是否存在
            var category = await _repository.GetCategoryByIdAsync(categoryId);
            if (category == null)
            {
                throw new Exception("分类不存在");
            }

            // 检查是否有子分类
            var childCount = await _repository.GetChildCategoryCountAsync(categoryId);
            if (childCount > 0)
            {
                throw new Exception("该分类下还有子分类，无法删除");
            }

            // 检查是否有关联图书
            if (await _repository.HasBookInCategoryAsync(categoryId))
            {
                throw new Exception("该分类下还有图书，无法删除");
            }

            var result = await _repository.DeleteCategoryAsync(categoryId);
            
            // 记录操作日志
            await _logService.LogCategoryDeleteAsync(category.CategoryName, category.CategoryName, operatorId);
            
            return result > 0;
        }
        catch (Exception ex)
        {
            await _logService.LogOperationFailureAsync("删除分类", "Librarian", operatorId, ex.Message);
            throw;
        }
    }
} 