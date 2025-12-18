fetch('https://localhost:7047/api/products/loai')
    .then(response => response.json())
    .then(result => {
        const data = result.data;
        console.log("LoaiSanPham:", data);
        var dsLoai = document.querySelector(".loaiSanPham");
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
        // chèn TRƯỚC divider
        dsLoai.insertAdjacentHTML("afterbegin", html);
    })
    .catch(err => console.error(err));

document.addEventListener("click", function (e) {
    if (e.target.classList.contains("loai-item")) {
        e.preventDefault();

        let loai = e.target.getAttribute("data-loai");

        fetch(`/api/customer/products?loai=${encodeURIComponent(loai)}`)
            .then(res => res.json())
            .then(result => {

                // ⬅️ GỌI HÀM RENDER
                renderSanPham(result.data);
            });
    }
});


