import { API_BASE_URL } from "../config/api.config-admin.js";

const id = document.getElementById("idSanPham").value;

if (id) {
    fetch(`${API_BASE_URL}/api/admin/sanpham/${id}`)
        .then(res => res.json())
        .then(sp => {
            $("#tenSanPham").val(sp.tenSanPham);
            $("#gia").val(sp.gia);
            $("#soLuong").val(sp.soLuongHienCon);
            $("#idLoaiSanPham").val(sp.idLoaiSanPham);
            $("#idCongTy").val(sp.idCongTy);

            if (sp.imageURL) {
                $("#previewImg")
                    .attr("src", sp.imageURL)
                    .show();
            }
        });
}

$("#frmSanPham").on("submit", function (e) {
    e.preventDefault();

    const formData = new FormData();
    formData.append("idSanPham", id);
    formData.append("tenSanPham", $("#tenSanPham").val());
    formData.append("gia", $("#gia").val());
    formData.append("soLuongHienCon", $("#soLuong").val());
    formData.append("idLoaiSanPham", $("#idLoaiSanPham").val());
    formData.append("idCongTy", $("#idCongTy").val());

    const file = $("#image")[0].files[0];
    if (file) formData.append("image", file);

    fetch(`${API_BASE_URL}/api/admin/sanpham/upsert`, {
        method: "POST",
        body: formData
    }).then(res => {
        if (res.ok) {
            Swal.fire({
                icon: "success",
                title: "Lưu thành công",
                timer: 1200,
                showConfirmButton: false
            }).then(() => location.href = "/Admin/SanPham");
        } else {
            Swal.fire("Lỗi", "Không thể lưu sản phẩm", "error");
        }
    });
});
