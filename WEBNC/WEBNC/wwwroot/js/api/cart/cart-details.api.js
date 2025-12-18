import { API_BASE } from "../../config/api.config.js";

export function addToCart(id, quantity) {
    return fetch(`${API_BASE}/cart/add`, {
        method: "POST",
        credentials: "include",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            idSanPham: id,
            soLuongTrongGio: quantity
        })
    });
}
