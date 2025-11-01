    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    if (id) {
        fetch(`/customer/home/SanPham?id=${id}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(sanpham => {
                let htmls = "";
                const data = sanpham.data;
                try {
                    images = JSON.parse(data.imageLienQuan);
                }catch(e){
                    console.error("Lỗi parse imageLienQuan:", e);
                }
                console.log('SanPham:', data);
                if (data) {
                    htmls = `
                            <div class="main-image">
                                <img src="/images/${data.imageURL}" alt="image main">
                            </div>
                            <div class="thumbnail-list">
                        `;

                    images.forEach((img, i) => {
                        htmls += `
                        <img src="/images/${img}" alt="Thumb ${i + 1}" 
                             class="thumb-img" 
                             style="width: 150px; height: 150px; object-fit: cover; border-radius: 8px; cursor: pointer;">
                        `;
                    });
                    htmls += `</div>`;
                    document.getElementById('sanPham').innerHTML = htmls;

                    // Khi click thumbnail đổi ảnh chính
                    const mainImg = document.querySelector(".main-image img");
                    document.querySelectorAll(".thumb-img").forEach(thumb => {
                        thumb.addEventListener("click", () => {
                            mainImg.src = thumb.src;
                        });
                    });

                    document.getElementById('maSP').innerHTML = "Mã sản phẩm : "+data.idSanPham;

                    document.querySelectorAll('.moTa').forEach(item => {
                        item.innerHTML = data.moTa;
                    });
                    const nameElement = document.querySelector('.tenSanPham'); 
                    if (nameElement) {
                        nameElement.innerHTML = data.tenSanPham; 
                    }
                    const priceElements = document.querySelectorAll('.gia');
                    const formattedPrice = data.gia.toLocaleString('vi-VN');
                    priceElements.forEach(element => {
                        element.innerHTML = `$${formattedPrice}`;
                    });
                }
            })
            .catch(error => {
                console.error('Error fetching product data:', error);
                document.getElementById('sanPham').innerHTML = 'Đã xảy ra lỗi khi tải dữ liệu sản phẩm.';
            });


        fetch('/customer/home/getall')
            .then(res => res.json())
            .then(sanpham => {
                const data = sanpham.data;
                let sanPhamLQ = '';
                const randomSanPham = data.sort(() => 0.5 - Math.random()).slice(0, 3);

                randomSanPham.forEach(product => {
                    sanPhamLQ += `
                        <div class="product-card product-item-inner-item position-relative">
                            <div class="product-details">
                                <a href="/Customer/Home/Details?id=${product.idSanPham}" class="view-icon">
                                    <img src="/images/${product.imageURL}" class="img-fluid w-100 rounded-top" alt="sản phẩm liên quan">
                                </a>
                            </div>
                            <div class="product-card-info">
                                <h4>${product.tenSanPham}</h4>
                                <p class="price">${product.gia.toLocaleString('vi-VN')} ₫</p>
                                <button class="add-to-cart-small">Thêm vào giỏ</button>
                            </div>
                        </div>
                        `;
                });

                document.getElementById('sanPhamLQ').innerHTML = sanPhamLQ;
            })
            .catch(err => console.error("Lỗi:", err));


        } else {
        console.warn('Product ID not found in URL parameters.');
        document.getElementById('sanPham').innerHTML = 'Không tìm thấy ID sản phẩm.';
}