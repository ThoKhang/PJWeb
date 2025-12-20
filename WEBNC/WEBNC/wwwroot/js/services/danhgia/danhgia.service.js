import { apiGetDanhGia, apiPostDanhGia } from "../../api/danhgia/danhgia.api.js";
import { renderDanhGia } from "../../ui/danhgia/renderDanhGia.js";

export async function loadDanhGia(idSanPham) {
    const data = await apiGetDanhGia(idSanPham);
    renderDanhGia(data);
}

export async function submitDanhGia(idSanPham) {
    let formData = new FormData();
    formData.append("idSanPham", idSanPham);
    formData.append("soSao", document.getElementById("soSao").value);
    formData.append("noiDung", document.getElementById("noiDung").value);

    const files = document.getElementById("hinhAnhs").files;
    for (let i = 0; i < files.length; i++) {
        formData.append("hinhAnhs", files[i]);
    }

    const res = await apiPostDanhGia(formData);

    if (res.ok) {
        Swal.fire({
            toast: true,
            position: "top-end",
            icon: "success",
            title: "Đánh giá thành công!",
            showConfirmButton: false,
            timer: 3000
        });

        document.getElementById("formDanhGia").reset();
        loadDanhGia(idSanPham);
    } else {
        Swal.fire("Không thể bình luận!");
    }
}
