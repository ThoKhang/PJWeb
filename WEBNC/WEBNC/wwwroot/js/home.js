fetch('https://localhost:7047/api/customer/products')
    .then(response => response.json())
    .then(sanpham => {
        const data = sanpham.data;
        console.log('SanPham:', data);
        let htmls = '';
        let listStart = '';
        data.forEach(product => {
            htmls += `
                    <div class="col-md-6 col-lg-4 col-xl-3">
                    
                        <div class="product-item rounded wow fadeInUp" data-wow-delay="0.3s">
                            <div class="product-item-inner border rounded">
                                <div class="product-item-inner-item">
                                    <img src="/images/${product.imageURL}" class="img-fluid w-100 rounded-top" alt="ảnh" style="width:auto;height:250px">
                                    <div class="product-new">New</div>
                                    <div class="product-details">
                                        <a href="/Customer/Home/Details?id=${product.idSanPham}"><i class="fa fa-eye fa-1x"></i></a>
                                    </div>
                                </div>
                                <div class="text-center rounded-bottom p-4">
                                    <a href="#" class="d-block mb-2">${product.loaiSanPham.tenLoaiSanPham}</a>
                                    <a href="#" class="d-block h4">${product.tenSanPham}</a>
                                    <del class="me-2 fs-5">${product.gia} </del>
                                    <span class="text-primary fs-5">${product.gia} VND</span>
                                </div>
                            </div>
                            <div class="product-item-add border border-top-0 rounded-bottom text-center p-4 pt-0">
                                <a href="#"
                                   class="btn btn-primary border-secondary rounded-pill py-2 px-4 mb-4">
                                    <i class="fas fa-shopping-cart me-2"></i> Add To Cart
                                </a>
                                <div class="d-flex justify-content-between align-items-center">
                                    <div class="d-flex">
                                        <i class="fas fa-star text-primary"></i>
                                        <i class="fas fa-star text-primary"></i>
                                        <i class="fas fa-star text-primary"></i>
                                        <i class="fas fa-star text-primary"></i>
                                        <i class="fas fa-star"></i>
                                    </div>
                                    <div class="d-flex">
                                        <a href="#"
                                           class="text-primary d-flex align-items-center justify-content-center me-3">
                                            <span class="rounded-circle btn-sm-square border">
                                                <i class="fas fa-random"></i></i>
                                        </a>
                                        <a href="#"
                                           class="text-primary d-flex align-items-center justify-content-center me-0">
                                            <span class="rounded-circle btn-sm-square border">
                                                <i class="fas fa-heart"></i>
                                        </a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    `;
            listStart +=
                `
                    <div class="productImg-item products-mini-item border">
                        <div class="row g-0">
                            <div class="col-5">
                                <div class="products-mini-img border-end h-100">
                                    <img src="/images/${product.imageURL}" class="img-fluid w-100 h-100" alt="Image">
                                    <div class="products-mini-icon rounded-circle bg-primary">
                                        <a href="#"><i class="fa fa-eye fa-1x text-white"></i></a>
                                    </div>
                                </div>
                            </div>
                            <div class="col-7">
                                <div class="products-mini-content p-3">
                                    <a href="#" class="d-block mb-2">${product.loaiSanPham.tenLoaiSanPham}</a>
                                    <a href="#" class="d-block h4">${product.tenSanPham}6</a>
                                    <del class="me-2 fs-5">${product.gia} </del>
                                    <span class="text-primary fs-5">${product.gia}VND</span>
                                </div>
                            </div>
                        </div>
                        <div class="products-mini-add border p-3">
                            <a href="#" class="btn btn-primary border-secondary rounded-pill py-2 px-4">
                                <i class="fas fa-shopping-cart me-2"></i> Thêm vào giỏ hàng
                            </a>
                            <div class="d-flex">
                                <a href="#"
                                   class="text-primary d-flex align-items-center justify-content-center me-3">
                                    <span class="rounded-circle btn-sm-square border">
                                        <i class="fas fa-random"></i></i>
                                </a>
                                <a href="#"
                                   class="text-primary d-flex align-items-center justify-content-center me-0">
                                    <span class="rounded-circle btn-sm-square border"><i class="fas fa-heart"></i>
                                </a>
                            </div>
                        </div>
                    </div>
                `
        });

        document.getElementById('ourSanPham').innerHTML = htmls;
        document.getElementById('newSanPham').innerHTML = htmls;
        document.getElementById('listStart').innerHTML = listStart;
    })
    .catch(error => {
        console.error('Lỗi:', error);
    });