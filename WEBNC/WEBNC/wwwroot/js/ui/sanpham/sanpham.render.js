export function initSanPhamTable(data) {

    if ($.fn.DataTable.isDataTable('#tblSanPham')) {
        $('#tblSanPham').DataTable().destroy();
    }

    $('#tblSanPham').DataTable({
        data: data,
        autoWidth: false,
        responsive: true,

        columns: [
            // 0️⃣ MÃ SẢN PHẨM
            {
                data: 'idSanPham'
            },

            // 1️⃣ TÊN SẢN PHẨM (RÚT NGẮN)
            {
                data: 'tenSanPham',
                render: function (text) {
                    if (!text) return '';
                    if (text.length > 25) {
                        return `
                            <span title="${text}">
                                ${text.substring(0, 25)}...
                            </span>
                        `;
                    }
                    return text;
                }
            },

            // 2️⃣ HÌNH ẢNH (SAU TÊN)
            {
                data: 'imageURL',
                orderable: false,
                searchable: false,
                render: function (img) {
                    if (!img)
                        return '<span class="text-muted">Không có ảnh</span>';

                    // Ảnh nằm trong wwwroot/images
                    const src = img.startsWith('/')
                        ? img
                        : `/images/${img}`;

                    return `
                        <img src="${src}"
                             style="width:70px;height:70px;
                                    object-fit:cover;
                                    border-radius:6px"
                             alt="Ảnh sản phẩm">
                    `;
                }
            },

            // 3️⃣ LOẠI SẢN PHẨM ✅ FIX
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

            // 6️⃣ SỐ LƯỢNG
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
                        <a href="/Admin/SanPham/Edit/${id}"
                           class="btn btn-sm btn-warning me-1">
                            <i class="fa-solid fa-pen-to-square"></i>
                        </a>

                        <button class="btn btn-sm btn-danger btn-delete"
                                data-id="${id}">
                            <i class="fa-solid fa-trash"></i>
                        </button>
                    `;
                }
            }
        ],

        // 🎯 CHỈNH ĐỘ RỘNG CỘT
        columnDefs: [
            { width: "100px", targets: 0 }, // Mã
            { width: "220px", targets: 1 }, // Tên
            { width: "100px", targets: 2 }, // Ảnh
            { width: "120px", targets: 3 }, // Loại
            { width: "120px", targets: 4 }, // Công ty
            { width: "120px", targets: 5 }, // Giá
            { width: "90px", targets: 6 }, // Số lượng
            { width: "120px", targets: 7 }  // Thao tác
        ]
    });
}
