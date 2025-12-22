import { API_BASE_URL } from "../../config/api.config-admin.js";
export function getAllLoaiSanPham() {
    return fetch(`${API_BASE_URL}/api/admin/loaisanpham`)
        .then(res => res.json());
}
export function deleteLoaiSanPham(id) {
    return fetch(`${API_BASE_URL}/api/admin/loaisanpham/${id}`, {
        method: "DELETE"
    });
}
