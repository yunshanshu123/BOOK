-- 注释掉的数据均未插入数据库，仅作为代码参考


-- /* ---------- 5. Building ---------- */
-- INSERT INTO Building (BuildingID, BuildingName, Address, TotalFloors,
--                       OpenHours, ManagerID, Remark)
-- VALUES ('BLD_TG', '总图书馆',
--         '上海市杨浦区四平路1239号 同济大学本部',7,
--         '08:00-22:30', 'LIB001', '主馆');

-- INSERT INTO Building (BuildingID, BuildingName, Address, TotalFloors,
--                       OpenHours, ManagerID, Remark)
-- VALUES ('BLD_DW', '德文图书馆',
--         '上海市杨浦区四平路1239号 同济大学本部', 2,
--         '09:00-18:00', 'LIB001', '外文馆');

-- /* ---------- 6. Bookshelf ---------- */
-- /* ==========================================================
--    批量生成 Bookshelf 记录
--    规则：
--      - 区域：大馆用 A~D 区，小馆用 A~B 区
--      - ShelfCode 格式：<两位楼层号><区字母>-<三位序号>
--        例：01A-001  表示 1 楼 A 区 第 1 号书架
-- ========================================================== */

-- SET SERVEROUTPUT ON
-- DECLARE
--     /* 公共过程，入参控制楼层/区/书柜数 */
--     PROCEDURE add_shelves (
--         p_building_id  IN VARCHAR2,
--         p_max_floor    IN PLS_INTEGER,
--         p_zone_count   IN PLS_INTEGER,
--         p_shelf_each   IN PLS_INTEGER )
--     IS
--     BEGIN
--         FOR f IN 1 .. p_max_floor LOOP            -- 楼层
--             FOR z IN 1 .. p_zone_count LOOP       -- 区域 A/B/...
--                 FOR s IN 1 .. p_shelf_each LOOP   -- 同区内书架序号
--                     INSERT INTO Bookshelf
--                           (BuildingID,
--                            ShelfCode,
--                            Floor,
--                            Zone)
--                     VALUES
--                           (p_building_id,
--                            LPAD(f, 2, '0') || CHR(64+z) || '-' || LPAD(s, 3, '0'),
--                            f,
--                            CHR(64+z) || '区');
--                 END LOOP;
--             END LOOP;
--         END LOOP;
--         DBMS_OUTPUT.PUT_LINE(
--           p_building_id || ': 完成 ' ||
--           p_max_floor*p_zone_count*p_shelf_each || ' 条记录');
--     END add_shelves;
-- BEGIN
--     /* 总图书馆：14 层，每层 4 区 (A~D)，每区 10 个书架 */
--     add_shelves('BLD_TG', 14, 4, 10);

--     /* 德文图书馆：2 层，每层 2 区 (A~B)，每区 5 个书架 */
--     add_shelves('BLD_DW', 2, 2, 5);

-- END;
-- /


-- test data
-- /* ---------- 9. Book ---------- */
INSERT INTO Book (Status, ShelfID, ISBN)
VALUES ('正常', 82, '9781594202667');