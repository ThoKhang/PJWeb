export function renderDetails(data) {

    let htmls = "";
    let images = [];

    try {
        if (data.imageLienQuan) {
            images = JSON.parse(data.imageLienQuan);
        }
    } catch (e) {
        console.error("Lỗi parse imageLienQuan:", e);
    }

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

    // thumbnail click
    const mainImg = document.querySelector(".main-image img");
    document.querySelectorAll(".thumb-img").forEach(thumb => {
        thumb.addEventListener("click", () => {
            mainImg.src = thumb.src;
        });
    });

    document.getElementById('maSP').innerHTML =
        "Mã sản phẩm : " + data.idSanPham;

    document.getElementById('thongSoSanPham').innerHTML =
        data.thongSoSanPham;

    document.querySelectorAll('.moTa').forEach(item => {
        item.innerHTML = data.moTa;
    });

    const nameElement = document.querySelector('.tenSanPham');
    if (nameElement) nameElement.innerHTML = data.tenSanPham;

    const formattedPrice = Number(data.gia).toLocaleString('vi-VN');
    document.querySelectorAll('.gia').forEach(element => {
        element.innerHTML = `${formattedPrice} ₫`;
    });
}
