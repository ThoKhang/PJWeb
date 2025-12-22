export async function apiGetDanhGia(idSanPham) {
    const res = await fetch(`/api/DanhGia?idSanPham=${idSanPham}`);
    return await res.json();
}

export async function apiPostDanhGia(formData) {
    return await fetch("/api/DanhGia", {
        method: "POST",
        body: formData
    });
}
