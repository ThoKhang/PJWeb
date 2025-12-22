import { getAllSanPham, deleteSanPham } from "../../api/sanpham/sanpham.api.js";

export function loadSanPham() {
    return getAllSanPham();
}

export function removeSanPham(id) {
    return deleteSanPham(id);
}