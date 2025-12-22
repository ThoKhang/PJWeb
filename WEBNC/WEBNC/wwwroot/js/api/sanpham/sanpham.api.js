import { API_BASE_URL } from "../../config/api.config-admin.js";

export function getAllSanPham() {
    return fetch(`${API_BASE_URL}/api/admin/sanpham`)
        .then(res => {
            if (!res.ok) {
                throw new Error("API SanPham lỗi");
            }
            return res.json();
        });
}

export function deleteSanPham(id) {
    return fetch(`${API_BASE_URL}/api/admin/sanpham/${id}`, {
        method: "DELETE"
    });
}
