const urlParams = new URLSearchParams(window.location.search);
let id = urlParams.get('id');

if (id) {
    document.addEventListener("click", (e) => {
        console.log("REAL TARGET:", e.target);
    });

    fetch(`https://localhost:7047/api/customer/products/${id}`)
        .then(response => response.json())
        .then(sanpham => {

            let htmls = "";
            const data = sanpham.data;
            if (!id) {
                id = data.idSanPham;
            }
            let images = [];
            try {
                if (data.imageLienQuan) {
                    images = JSON.parse(data.imageLienQuan);
                }
            } catch (e) {
                console.error("Lỗi parse imageLienQuan:", e);
            }

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
                             class="thumb-img" style="width:150px;height:150px;
                             object-fit:cover;border-radius:8px;cursor:pointer;">
                    `;
                });

                htmls += `</div>`;
                document.getElementById('sanPham').innerHTML = htmls;

                // --- thumbnail click ---
                const mainImg = document.querySelector(".main-image img");
                document.querySelectorAll(".thumb-img").forEach(thumb => {
                    thumb.addEventListener("click", () => {
                        mainImg.src = thumb.src;
                    });
                });

                document.getElementById('maSP').innerHTML = "Mã sản phẩm : " + data.idSanPham;
                document.getElementById('thongSoSanPham').innerHTML = data.thongSoSanPham;

                document.querySelectorAll('.moTa').forEach(item => {
                    item.innerHTML = data.moTa;
                });

                const nameElement = document.querySelector('.tenSanPham');
                if (nameElement) nameElement.innerHTML = data.tenSanPham;

                const formattedPrice = Number(data.gia).toLocaleString('vi-VN');
                document.querySelectorAll('.gia').forEach(element => {
                    element.innerHTML = `${formattedPrice} ₫`;
                });

                //tăng giảm số lượng
                const minusBtn = document.querySelector('.minus');
                const plusBtn = document.querySelector('.plus');
                const quantityInput = document.querySelector('.qty');

                if (minusBtn && plusBtn && quantityInput) {

                    minusBtn.addEventListener("click", () => {
                        let val = Number(quantityInput.value);
                        if (val > 1) quantityInput.value = val - 1;
                    });

                    plusBtn.addEventListener("click", () => {
                        let val = Number(quantityInput.value);
                        quantityInput.value = val + 1;
                    });

                } else {
                    console.warn("Không tìm thấy .minus .plus hoặc .qty");
                }

                // thêm vào giỏ hàng
                const btnAddToCart = document.querySelector('.btn-cart');

                if (btnAddToCart && quantityInput) {

                    btnAddToCart.addEventListener("click", async () => {

                        const quantity = Number(quantityInput.value) || 1;

                        const res = await fetch("https://localhost:7047/api/cart/add", {
                            method: "POST",
                            credentials: "include",        // gửi cookie identity
                            headers: { "Content-Type": "application/json" },
                            body: JSON.stringify({
                                idSanPham: id,
                                soLuongTrongGio: quantity
                            })
                        });

                        // Chưa đăng nhập
                        if (res.status === 401) {
                            const returnUrl = `/Customer/Home/Details?id=${encodeURIComponent(id)}`;
                            window.location.href =
                                `/Identity/Account/Login?returnUrl=${encodeURIComponent(returnUrl)}`;
                            return;
                        }

                        // Lỗi server
                        if (!res.ok) {
                            Swal.fire({
                                icon: "error",
                                title: "Thêm giỏ hàng thất bại",
                                text: "Vui lòng thử lại!",
                            });
                            return;
                        }

                        // Thành công
                        Swal.fire({
                            position: "top-end",
                            icon: "success",
                            title: "Đã thêm vào giỏ hàng",
                            showConfirmButton: false,
                            timer: 1500
                        });

                    });

                } else {
                    console.warn("Không tìm thấy .btn-cart hoặc .qty");
                }

                // --- CHỨC NĂNG MUA NGAY (CẬP NHẬT MỚI: NHẬP SĐT + ĐỊA CHỈ) ---
                const btnBuyNow = document.querySelector('.btn-buy');

                if (btnBuyNow && quantityInput) {
                    btnBuyNow.addEventListener("click", () => {
                        // 1. Lấy số lượng từ input
                        const quantity = Number(quantityInput.value) || 1;

                        // Hàm con thực hiện gọi API (hỗ trợ gọi lại khi có SĐT/Địa chỉ)
                        const executeBuyNow = async (phone = null, address = null) => {
                            try {
                                const payload = {
                                    soLuong: quantity,
                                    sdt: phone,
                                    diaChi: address // Gửi kèm địa chỉ nếu có
                                };

                                const res = await fetch(`https://localhost:7047/api/customer/products/${id}`, {
                                    method: "POST",
                                    credentials: "include",
                                    headers: { "Content-Type": "application/json" },
                                    body: JSON.stringify(payload)
                                });

                                if (res.status === 401) {
                                    const returnUrl = `/Customer/Home/Details?id=${encodeURIComponent(id)}`;
                                    window.location.href = `/Identity/Account/Login?returnUrl=${encodeURIComponent(returnUrl)}`;
                                    return;
                                }

                                const data = await res.json();

                                // TRƯỜNG HỢP 1: Server yêu cầu bổ sung thông tin (SĐT hoặc Địa chỉ)
                                if (data.success === false && data.requiresInfo === true) {

                                    // Tạo form HTML động tùy theo cái nào thiếu
                                    let formHtml = '';
                                    if (data.missingSdt) {
                                        formHtml += `
                            <div style="margin-bottom: 15px; text-align: left;">
                                <label style="font-weight:bold;">Số điện thoại <span style="color:red">*</span></label>
                                <input id="swal-input-sdt" class="swal2-input" style="margin: 5px 0; width: 100%;" placeholder="Nhập số điện thoại (09xxxxxxxx)">
                            </div>`;
                                    }
                                    if (data.missingAddress) {
                                        formHtml += `
                            <div style="text-align: left;">
                                <label style="font-weight:bold;">Địa chỉ nhận hàng <span style="color:red">*</span></label>
                                <input id="swal-input-diachi" class="swal2-input" style="margin: 5px 0; width: 100%;" placeholder="Nhập số nhà, tên đường, phường/xã...">
                            </div>`;
                                    }

                                    const { value: formValues } = await Swal.fire({
                                        title: 'Thông tin giao hàng',
                                        html: formHtml,
                                        focusConfirm: false,
                                        showCancelButton: true,
                                        confirmButtonText: 'Xác nhận mua',
                                        cancelButtonText: 'Hủy',
                                        preConfirm: () => {
                                            let sdtVal = phone; // Giữ giá trị cũ nếu không cần nhập
                                            let diaChiVal = address;

                                            // Validate SĐT nếu form có hiện ô nhập SĐT
                                            if (data.missingSdt) {
                                                sdtVal = document.getElementById('swal-input-sdt').value;
                                                if (!sdtVal || !/^\d{10,11}$/.test(sdtVal)) {
                                                    Swal.showValidationMessage('Số điện thoại không hợp lệ (phải 10-11 số)');
                                                    return false;
                                                }
                                            }

                                            // Validate Địa chỉ nếu form có hiện ô nhập địa chỉ
                                            if (data.missingAddress) {
                                                diaChiVal = document.getElementById('swal-input-diachi').value;
                                                if (!diaChiVal || diaChiVal.trim().length < 5) {
                                                    Swal.showValidationMessage('Vui lòng nhập địa chỉ cụ thể hơn');
                                                    return false;
                                                }
                                            }

                                            return { sdt: sdtVal, diaChi: diaChiVal };
                                        }
                                    });

                                    // Nếu người dùng nhập xong và bấm OK -> Gọi lại API
                                    if (formValues) {
                                        executeBuyNow(formValues.sdt, formValues.diaChi);
                                    }
                                }
                                // TRƯỜNG HỢP 2: Thành công
                                else if (data.success) {
                                    Swal.fire({
                                        icon: 'success',
                                        title: 'Đặt hàng thành công!',
                                        text: 'Đang chuyển đến lịch sử đơn hàng...',
                                        showConfirmButton: false,
                                        timer: 1500
                                    }).then(() => {
                                        window.location.href = data.redirectUrl;
                                    });
                                }
                                // TRƯỜNG HỢP 3: Lỗi khác (Hết hàng, lỗi DB...)
                                else {
                                    Swal.fire({ icon: "error", title: "Lỗi", text: data.message || "Đã có lỗi xảy ra" });
                                }
                            } catch (error) {
                                console.error("Lỗi mua ngay:", error);
                                Swal.fire({ icon: "error", title: "Lỗi hệ thống", text: "Không thể kết nối đến server." });
                            }
                        };

                        // Gọi hàm thực thi lần đầu
                        executeBuyNow();
                    });
                } else {
                    console.warn("Không tìm thấy nút .btn-buy hoặc ô nhập số lượng");
                }
            }
        })
        .catch(error => {
            console.error('Error fetching product data:', error);
            document.getElementById('sanPham').innerHTML = 'Đã xảy ra lỗi khi tải dữ liệu sản phẩm.';
        });

    // 3 sản phẩm liên quan 
    fetch('https://localhost:7047/api/customer/products')
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
                                <img src="/images/${product.imageURL}" class="img-fluid w-100 rounded-top" alt="sản phẩm liên quan" style="height:180px">
                            </a>
                        </div>
                        <div class="product-card-info">
                            <h4 style="height:45px;overflow:hidden;-webkit-line-clamp:2;-webkit-box-orient:vertical;">${product.tenSanPham}</h4>
                            <p class="price">${Number(product.gia).toLocaleString('vi-VN')} ₫</p>
                            <button class="add-to-cart-small">Thêm vào giỏ</button>
                        </div>
                    </div>
                `;
            });

            document.getElementById('sanPhamLQ').innerHTML = sanPhamLQ;
        });

    // Top sản phẩm 
    fetch('https://localhost:7047/api/customer/products/top')
        .then(response => response.json())
        .then(result => {
            const data = result.data;
            let html = '';
            data.forEach((sp, i) => {
                const giaGiam = Number(sp.gia) * 0.86;
                html += `
                    <div class="featured-item">
                        <a href="/Customer/Home/Details?id=${sp.idSanPham}" class="view-icon">
                            <img src="/images/${sp.imageURL}" alt="${sp.tenSanPham}">
                        </a>
                        <div class="featured-info">
                            <h4>${sp.tenSanPham}</h4>
                            <p class="price">
                                ${giaGiam.toLocaleString('vi-VN')}₫ 
                                <span class="old-price">${Number(sp.gia).toLocaleString('vi-VN')}₫</span>
                            </p>
                            <p class="rating">★★★★☆</p>
                        </div>
                    </div>
                `;
            });
            document.getElementById('sanPhamNoiBat').innerHTML = html;

        });

} else {
    document.getElementById('sanPham').innerHTML = 'Không tìm thấy ID sản phẩm.';
}
