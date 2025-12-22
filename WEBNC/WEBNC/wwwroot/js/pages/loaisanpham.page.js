import { loadLoaiSanPham, removeLoaiSanPham } from "../services/loaisanpham/loaisanpham.service.js";
import { initLoaiSanPhamTable } from "../ui/loaisanpham/loaisanpham.render.js";
$(document).ready(async function () {
    const data = await loadLoaiSanPham();
    initLoaiSanPhamTable(data);

    $(document).on("click", ".btn-delete", async function () {
        const id = $(this).data("id");
        const result = await Swal.fire({
            title: 'Xác nhận xóa?',
            text: 'Bạn có chắc muốn xóa loại sản phẩm này không?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy',
            confirmButtonColor: '#d33',
            cancelButtonColor: '#3085d6'
        });
        if (!result.isConfirmed) return;
        try {
            const res = await removeLoaiSanPham(id);
            if (res.ok) {
                Swal.fire({
                    icon: 'success',
                    title: 'Đã xóa',
                    text: 'Xóa loại sản phẩm thành công',
                    timer: 1500,
                    showConfirmButton: false
                });
                $('#tblLoaiSanPham').DataTable().ajax.reload();
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Thất bại',
                    text: 'Không thể xóa loại sản phẩm'
                });
            }
        } catch (err) {
            console.error(err);
            Swal.fire({
                icon: 'error',
                title: 'Lỗi',
                text: 'Có lỗi xảy ra khi xóa'
            });
        }
    });
});
