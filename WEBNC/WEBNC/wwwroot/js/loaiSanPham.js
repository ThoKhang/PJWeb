fetch('https://localhost:7047/api/products/loai')
    .then(response => response.json())
    .then(result => {
        const data = result.data;
        console.log("LoaiSanPham:", data);
        var dsLoai = document.querySelector(".loaiSanPham");
        let html = "";
        data.forEach(loai => {
            html += `
                <li><a class="dropdown-item py-2" href="#"><i class="fas fa-mobile-alt me-2 text-primary"></i> ${loai.tenLoaiSanPham}</a></li>
            `;
        });
        // chèn TRƯỚC divider
        dsLoai.insertAdjacentHTML("afterbegin", html);
    })
    .catch(err => console.error(err));