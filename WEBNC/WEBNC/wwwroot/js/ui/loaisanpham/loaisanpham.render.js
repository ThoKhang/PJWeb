export function initLoaiSanPhamTable(data) {
    $('#tblLoaiSanPham').DataTable({
        data: data,
        columns: [
            { data: 'idLoaiSanPham' },
            { data: 'tenLoaiSanPham' },
            {
                data: 'idLoaiSanPham',
                orderable: false,
                searchable: false,
                render: (id) => `
                    <a href="/Admin/LoaiSanPham/Edit/${id}" class="btn btn-sm btn-warning me-1" title="Sửa">
                        <i class="fa-solid fa-pen-to-square"></i>
                    </a>
                    <button class="btn btn-sm btn-danger btn-delete" data-id="${id}" title="Xóa">
                        <i class="fa-solid fa-trash"></i>
                    </button>
                `
            }
        ]
    });
}
