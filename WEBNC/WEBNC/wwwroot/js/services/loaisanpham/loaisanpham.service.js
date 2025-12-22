import { getAllLoaiSanPham, deleteLoaiSanPham } from "../../api/loaisanpham/loaisanpham.api.js";
export function loadLoaiSanPham() {
    return getAllLoaiSanPham();
}
export function removeLoaiSanPham(id) {
    return deleteLoaiSanPham(id);
}
