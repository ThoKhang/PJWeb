import { API_BASE_URL } from "../config/api.config-admin.js";

$(document).ready(async function () {

    const id = $("#idLoaiSanPham").val()?.trim();

    if (id) {
        const res = await fetch(`${API_BASE_URL}/api/admin/loaisanpham/${id}`);

        if (!res.ok) {
            Swal.fire("Lỗi", "Không tìm thấy loại sản phẩm", "error");
            return;
        }

        const item = await res.json();
        $("#tenLoaiSanPham").val(item.tenLoaiSanPham);
    }
    $("#frmLoaiSanPham").on("submit", async function (e) {
        e.preventDefault();

        const payload = {
            idLoaiSanPham: $("#idLoaiSanPham").val()?.trim(),
            tenLoaiSanPham: $("#tenLoaiSanPham").val()
        };

        const res = await fetch(`${API_BASE_URL}/api/admin/loaisanpham`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });

        if (!res.ok) {
            Swal.fire("Lỗi", "Không thể lưu dữ liệu", "error");
            return;
        }

        const result = await res.json();

        Swal.fire({
            icon: "success",
            title: result.mode === "create"
                ? "Thêm thành công!"
                : "Cập nhật thành công!",
            timer: 1200,
            showConfirmButton: false
        }).then(() => {
            window.location.href = "/Admin/LoaiSanPham";
        });
    });
});
