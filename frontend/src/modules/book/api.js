
import http from '@/services/http.js'

export function getBooks(keyword) {
  return http.get('/book/search', {
    params: { keyword }
})
}




export function getCommentsByISBN(ISBN) {
  return http.get('/comment/search', {
    params: { ISBN }
  })
}

export function addComment(commentData) {
  return http.post('/comment/add', commentData)
}

export function getCategoryTree() {
  return http.get('/Category/tree', {
    withToken: true // 确保每个请求都带上 Token
  });
}

export function addCategory(data) {
  return http.post('/Category', data)
}
export function updateCategory(data) {
  return http.put('/Category', data)
}
export function deleteCategory(id, operatorId) {
  return http.delete(`/Category/${id}?operatorId=${operatorId}`)
}

export function getBooksBookShelf(keyword) {
return http.get('/bookshelf/search_book_which_shelf', {
    params: { keyword }
})
}

export function getShelf(keyword) {
return http.get('/bookshelf/search_bookshelf', {
    params: { keyword }
})
}
export function addShelf(buildingid, shelfcode, floor, zone) {
  return http.post('/bookshelf/add_bookshelf', {
    buildingid: Number(buildingid),
    shelfcode: shelfcode,
    floor: Number(floor),
    zone: zone
  })
}

export function deleteShelf(shelfId) {
  return http.delete(`/bookshelf/delete/${shelfId}`);
}

export function checkShelfHasBooks(shelfId) {
  return http.get(`/bookshelf/has-books/${shelfId}`);
}

// 检查书架是否存在
export function checkShelfExists(buildingId, shelfCode, floor, zone) {
  return http.get('/bookshelf/check-shelf-exists', {
    params: { buildingId, shelfCode, floor, zone }
  });
}

export function returnBook(bookId, shelfId) {
  return http.post('/bookshelf/return-book', { 
    bookId: Number(bookId),
    shelfId: Number(shelfId)
  });
}
// 获取书架ID
export function findShelfId(buildingId, shelfCode, floor, zone) {
  return http.get('/bookshelf/find-shelf-id', {
    params: { 
      buildingId, 
      shelfCode, 
      floor, 
      zone 
    }
  });
}

export function borrowBook(bookId) {
  return http.post('/bookshelf/borrow-book', { bookId })
}

// ---------- 查询单册 ----------
export function getBookById(bookId) {
  return http.get(`/Book/${bookId}`)
}

export function getBookByBarcode(barcode) {
  return http.get(`/Book/by-barcode/${encodeURIComponent(barcode)}`)
}

// ---------- 状态流转：按 BookID ----------
export function borrowBookById(bookId) {
  return http.patch(`/Book/${bookId}/borrow`)
}

export function offShelfBookById(bookId) {
  return http.patch(`/Book/${bookId}/off-shelf`)
}

export function returnBookById(bookId) {
  return http.patch(`/Book/${bookId}/return`)
}

export function onShelfBookById(bookId) {
  return http.patch(`/Book/${bookId}/on-shelf`)
}

// ---------- 状态流转：按条码 ----------
export function borrowBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/borrow`)
}

export function offShelfBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/off-shelf`)
}

export function returnBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/return`)
}

export function onShelfBookByBarcode(barcode) {
  return http.patch(`/Book/by-barcode/${encodeURIComponent(barcode)}/on-shelf`)
}