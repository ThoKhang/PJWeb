import { API_BASE } from "../../config/api.config.js";

export function getLoaiSanPham() {
    return fetch(`${API_BASE}/products/loai`)
        .then(res => res.json());
}

export function getSanPhamByLoai(loai) {
    return fetch(`${API_BASE}/customer/products?loai=${encodeURIComponent(loai)}`)
        .then(res => res.json());
}
