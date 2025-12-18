import { API_BASE } from "../../config/api.config.js";

export function getProductById(id) {
    return fetch(`${API_BASE}/customer/products/${id}`)
        .then(response => response.json());
}

export function getAllProducts() {
    return fetch(`${API_BASE}/customer/products`)
        .then(res => res.json());
}

export function getTopProducts() {
    return fetch(`${API_BASE}/customer/products/top`)
        .then(response => response.json());
}
