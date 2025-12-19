import { getLoaiSanPham, getSanPhamByLoai } from "../api/product/product-loai.api.js";
import { renderLoaiSanPham, renderSanPham } from "../ui/product/loai.render.js";

getLoaiSanPham()
    .then(result => {
        console.log("LoaiSanPham:", result.data);
        renderLoaiSanPham(result.data);
    })
    .catch(err => console.error(err));

document.addEventListener("click", function (e) {
    if (e.target.classList.contains("loai-item")) {
        e.preventDefault();

        const loai = e.target.getAttribute("data-loai");

        getSanPhamByLoai(loai)
            .then(result => {
                renderSanPham(result.data);
            });
    }
});
