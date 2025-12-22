import { loadDanhGia, submitDanhGia } from "../services/danhgia/danhgia.service.js";

document.addEventListener("DOMContentLoaded", () => {
    const input = document.getElementById("idSanPham");
    if (!input) return;

    const idSanPham = input.value;

    loadDanhGia(idSanPham);

    document
        .getElementById("formDanhGia")
        .addEventListener("submit", function (e) {
            e.preventDefault();
            submitDanhGia(idSanPham);
        });
});
