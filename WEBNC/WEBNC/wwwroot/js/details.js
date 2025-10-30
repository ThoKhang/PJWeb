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
                const data = sanpham.data;
                console.log('SanPham:', data);
                if (data) {
                    let htmls = `
                            <div class="main-image">
                                <img src="/images/${data.imageURL}" alt="iPhone 14 Pro Max">
                            </div>
                            <div class="thumbnail-list">
                                <img src="/images/${data.imageURL}" alt="Thumb 1" class="active">
                                <img src="/images/${data.imageURL}" alt="Thumb 2">
                                <img src="/images/${data.imageURL}" alt="Thumb 3">
                                <img src="/images/${data.imageURL}" alt="Thumb 4">
                            </div>
                        `;
                    document.getElementById('sanPham').innerHTML = htmls;
                    document.querySelector('.moTa').innerHTML = data.moTa;
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
        } else {
        console.warn('Product ID not found in URL parameters.');
    document.getElementById('sanPham').innerHTML = 'Không tìm thấy ID sản phẩm.';
        }