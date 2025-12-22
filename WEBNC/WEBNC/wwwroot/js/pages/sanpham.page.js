import { getAllSanPham, deleteSanPham } from "../api/sanpham/sanpham.api.js";
import { initSanPhamTable } from "../ui/sanpham/sanpham.render.js";

$(document).ready(async function () {
    const data = await getAllSanPham();
    initSanPhamTable(data);

    $(document).on("click", ".btn-delete", async function () {
        const id = $(this).data("id");

        const result = await Swal.fire({
            title: 'Xác nhận xóa?',
            text: 'Bạn có chắc muốn xóa sản phẩm này không?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy',
            confirmButtonColor: '#d33'
        });

        if (!result.isConfirmed) return;

        const res = await deleteSanPham(id);
        if (res.ok) {
            Swal.fire({
                icon: 'success',
                title: 'Đã xóa',
                timer: 1500,
                showConfirmButton: false
            });
            $('#tblSanPham').DataTable().clear().rows.add(await getAllSanPham()).draw();
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Không thể xóa'
            });
        }
    });
});
