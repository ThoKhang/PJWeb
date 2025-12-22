// Đường dẫn: /js/api/doitra/doitra.api.js

export async function apiPostYeuCauDoiTra(formData) {
    // Gọi đến Action GuiYeuCau trong YeuCauDoiTraController
    const response = await fetch('/api/YeuCauDoiTra', {
        method: 'POST',
        body: formData
    });
    return response;
}

export async function apiGetLichSuDoiTra() {
    return await fetch('/api/YeuCauDoiTra/get-my-requests', {
        method: 'GET'
    });
}


export async function apiDeleteYeuCauDoiTra(idYeuCau) {
    return await fetch(`/api/YeuCauDoiTra/${idYeuCau}`, {
        method: 'DELETE'
    });
}