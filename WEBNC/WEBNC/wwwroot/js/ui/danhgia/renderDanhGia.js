export function renderDanhGia(data) {
    const container = document.getElementById("danhSachDanhGia");
    container.innerHTML = "";

    const danhGiaList = data.danhGiaList || [];

    if (danhGiaList.length === 0) {
        document.getElementById("diemTrungBinh").innerText = "0 ★";
        container.innerHTML = "<p class='text-muted'>Chưa có đánh giá nào.</p>";
        return;
    }

    document.getElementById("diemTrungBinh").innerText =
        data.diemTrungBinh + " ★";

    danhGiaList.forEach(dg => {
        let html = `
            <div class="border rounded p-3 mb-3">
                <div class="d-flex justify-content-between">
                    <strong>${dg.userName ?? "Ẩn danh"}</strong>
                    <span class="text-warning">${dg.soSao} ★</span>
                </div>
                <p class="mt-2 mb-1">${dg.noiDung ?? ""}</p>
                <small class="text-muted">
                    ${new Date(dg.ngayDanhGia).toLocaleString()}
                </small>
        `;

        if (dg.hinhAnhs && dg.hinhAnhs.length > 0) {
            html += `<div class="mt-2">`;
            dg.hinhAnhs.forEach(imgUrl => {
                html += `
                    <img src="${imgUrl}"
                         class="rounded me-2 mb-2"
                         style="width:80px;height:80px;object-fit:cover" />
                `;
            });
            html += `</div>`;
        }

        html += `</div>`;
        container.innerHTML += html;
    });
}
