export function initLoaiSanPhamTable(data) {
    const $table = $('#tblLoaiSanPham');

    // Nếu đã có DataTable thì phá hủy trước
    if ($.fn.DataTable.isDataTable($table)) {
        $table.DataTable().clear().destroy();
    }

    $table.DataTable({
        data: data,
        autoWidth: false,
        responsive: true,

        columns: [
            // Mã loại
            { data: 'idLoaiSanPham' },

            // Tên loại (in đậm, nếu dài thì cắt bớt)
            {
                data: 'tenLoaiSanPham',
                render: function (text) {
                    if (!text) return '';
                    const full = text;
                    if (text.length > 40) {
                        text = text.substring(0, 40) + '...';
                    }
                    return `<span class="fw-semibold" title="${full}">${text}</span>`;
                }
            },

            // Thao tác
            {
                data: 'idLoaiSanPham',
                orderable: false,
                searchable: false,
                render: (id) => `
                    <div class="d-flex justify-content-center gap-2">
                        <a href="/Admin/LoaiSanPham/Edit/${id}"
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
                `
            }
        ],

        columnDefs: [
            { width: "20%", targets: 0 },
            { width: "55%", targets: 1 },
            { width: "25%", targets: 2 }
        ],

        // Bố cục + Việt hóa DataTables (giống bảng sản phẩm)
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

    // Placeholder cho ô search
    $('#tblLoaiSanPham_filter input')
        .attr('placeholder', 'Tìm theo mã, tên loại sản phẩm...')
        .css('width', '240px');
}
