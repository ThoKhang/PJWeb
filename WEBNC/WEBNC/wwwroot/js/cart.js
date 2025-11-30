document.addEventListener("DOMContentLoaded", function () {

    fetch("/api/cart", { credentials: "include" })
        .then(res => {
            if (res.status === 401) {
                window.location.href = "/Identity/Account/Login?returnUrl=/Customer/Cart/Index";
                return;
            }
            return res.json();
        })
        .then(json => {
            if (!json) return;

            const items = json.data || [];
            const tbody = document.getElementById("cart-body");
            tbody.innerHTML = "";

            let subtotal = 0;

            items.forEach(item => {

                const total = item.sanPham.gia * item.soLuongTrongGio;
                subtotal += total;

                const tr = document.createElement("tr");
                tr.innerHTML = `
                    <th scope="row">
                        <p class="mb-0 py-4">${item.sanPham.tenSanPham}</p>
                    </th>

                    <td>
                        <p class="mb-0 py-4">${item.sanPham.loaiSanPham?.tenLoaiSanPham ?? ""}</p>
                    </td>

                    <td>
                        <p class="mb-0 py-4">${item.sanPham.gia.toLocaleString()} ₫</p>
                    </td>

                    <td>
                        <div class="input-group quantity py-4" style="width: 100px;">
                            <div class="input-group-btn">
                                <button class="btn btn-sm btn-minus rounded-circle bg-light border">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>

                            <input type="text" class="form-control form-control-sm text-center border-0"
                                   value="${item.soLuongTrongGio}">

                            <div class="input-group-btn">
                                <button class="btn btn-sm btn-plus rounded-circle bg-light border">
                                    <i class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                    </td>

                    <td>
                        <p class="mb-0 py-4">${total.toLocaleString()} ₫</p>
                    </td>

                    <td class="py-4">
                        <button class="btn btn-md rounded-circle bg-light border">
                            <i class="fa fa-times text-danger"></i>
                        </button>
                    </td>
                `;

                tbody.appendChild(tr);
            });

            const shipping = subtotal > 0 ? 30000 : 0;
            const totalAll = subtotal + shipping;

            document.querySelector(".subtotal-value").textContent = subtotal.toLocaleString() + " ₫";
            document.querySelector(".shipping-value").textContent = shipping.toLocaleString() + " ₫";
            document.querySelector(".total-value").textContent = totalAll.toLocaleString() + " ₫";
        });

});