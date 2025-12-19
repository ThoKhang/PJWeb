export function renderLoaiSanPham(data) {
    const dsLoai = document.querySelector(".loaiSanPham");
    if (!dsLoai) return;

    let html = "";
    data.forEach(loai => {
        html += `
            <li>
                <a class="dropdown-item py-2 loai-item"
                   data-loai="${loai.tenLoaiSanPham}"
                   href="#">
                    <i class="fas fa-mobile-alt me-2 text-primary"></i>
                    ${loai.tenLoaiSanPham}
                </a>
            </li>
        `;
    });

    dsLoai.insertAdjacentHTML("afterbegin", html);
}

export function renderSanPham(data) {
    let html = "";

    data.forEach(sp => {
        html += `
            <div class="product-card product-item-inner-item position-relative">
                <div class="product-details">
                    <a href="/Customer/Home/Details?id=${sp.idSanPham}">
                        <img src="/images/${sp.imageURL}"
                             class="img-fluid w-100 rounded-top"
                             style="height:180px"
                             alt="${sp.tenSanPham}">
                    </a>
                </div>
                <div class="product-card-info">
                    <h4 style="height:45px;overflow:hidden;">
                        ${sp.tenSanPham}
                    </h4>
                    <p class="price">
                        ${Number(sp.gia).toLocaleString('vi-VN')} ₫
                    </p>
                </div>
            </div>
        `;
    });

    document.getElementById("listStart").innerHTML = html;
}
