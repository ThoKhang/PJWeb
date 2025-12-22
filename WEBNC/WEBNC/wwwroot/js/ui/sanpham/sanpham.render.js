export function initSanPhamTable(data) {
    const $table = $('#tblSanPham');

    // Nếu đã có DataTable thì phá hủy trước
    if ($.fn.DataTable.isDataTable($table)) {
        $table.DataTable().clear().destroy();
    }

    const dt = $table.DataTable({
        data: data,
        autoWidth: false,
        responsive: true,

        columns: [
            // 0️⃣ MÃ SẢN PHẨM
            {
                data: 'idSanPham'
            },

            // 1️⃣ TÊN SẢN PHẨM (RÚT NGẮN, ĐẬM)
            {
                data: 'tenSanPham',
                render: function (text) {
                    if (!text) return '';
                    const full = text;
                    if (text.length > 30) {
                        text = text.substring(0, 30) + '...';
                    }
                    return `<span class="fw-semibold" title="${full}">${text}</span>`;
                }
            },

            // 2️⃣ HÌNH ẢNH
            {
                data: 'imageURL',
                orderable: false,
                searchable: false,
                render: function (img) {
                    if (!img)
                        return '<span class="text-muted" style="font-size:12px;">Không có ảnh</span>';

                    const src = img.startsWith('/')
                        ? img
                        : `/images/${img}`;

                    return `
                        <img src="${src}"
                             class="thumb"
                             alt="Ảnh sản phẩm">
                    `;
                }
            },

            // 3️⃣ LOẠI SẢN PHẨM
            {
                data: 'loaiSanPham',
                render: function (x) {
                    return x ? x.tenLoaiSanPham : '';
                }
            },

            // 4️⃣ CÔNG TY
            {
                data: 'congTy',
                render: function (x) {
                    return x ? x.tenCongTy : '';
                }
            },

            // 5️⃣ GIÁ
            {
                data: 'gia',
                render: function (x) {
                    return x
                        ? x.toLocaleString('vi-VN') + ' ₫'
                        : '0 ₫';
                }
            },

            // 6️⃣ TỒN KHO
            {
                data: 'soLuongHienCon'
            },

            // 7️⃣ THAO TÁC
            {
                data: 'idSanPham',
                orderable: false,
                searchable: false,
                render: function (id) {
                    return `
                        <div class="d-flex justify-content-center gap-2">
                            <a href="/Admin/SanPham/Edit/${id}"
                               class="btn-mini"
                               title="Chỉnh sửa">
                                <i class="fa-solid fa-pen-to-square"></i>
                            </a>

                            <button class="btn-mini btn-mini-danger btn-delete"
                                    data-id="${id}"
                                    title="Xóa">
                                <i class="fa-solid fa-trash"></i>
                            </button>
                        </div>
                    `;
                }
            }
        ],

        columnDefs: [
            { width: "90px", targets: 0 },  // Mã
            { width: "230px", targets: 1 }, // Tên
            { width: "90px", targets: 2 },  // Ảnh
            { width: "120px", targets: 3 }, // Loại
            { width: "120px", targets: 4 }, // Công ty
            { width: "120px", targets: 5 }, // Giá
            { width: "80px", targets: 6 },  // Tồn kho
            { width: "130px", targets: 7 }  // Thao tác
        ],

        // 🌈 Bố cục + Việt hóa DataTables
        dom:
            "<'dt-top d-flex flex-wrap justify-content-between align-items-center'<'dt-left'l><'dt-right'f>>" +
            "rt" +
            "<'dt-bottom d-flex flex-wrap justify-content-between align-items-center'<'dt-left'i><'dt-right'p>>",

        language: {
            lengthMenu: "Hiển thị _MENU_ dòng",
            search: "Tìm kiếm:",
            info: "Hiển thị _START_ đến _END_ của _TOTAL_ dòng",
            infoEmpty: "Không có dữ liệu",
            infoFiltered: "(lọc từ _MAX_ dòng)",
            zeroRecords: "Không tìm thấy kết quả phù hợp",
            paginate: {
                first: "Đầu",
                last: "Cuối",
                next: "›",
                previous: "‹"
            },
            loadingRecords: "Đang tải...",
            processing: "Đang xử lý..."
        }
    });

    // Placeholder + width cho ô search
    $('#tblSanPham_filter input')
        .attr('placeholder', 'Tìm theo mã, tên, công ty...')
        .css('width', '240px');
}
