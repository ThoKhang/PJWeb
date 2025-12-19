document.addEventListener("DOMContentLoaded", function () {

    const params = new URLSearchParams(window.location.search);
    if (params.get("cancel") === "1") {
        const reason = params.get("msg") || "Giao dịch đã hủy";
        alert(reason);
    }

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
                    <td class="py-4 text-center">
                        <input type="checkbox" class="cart-check" checked>
                    </td>

                    <td scope="row">
                        <div class="cart-item-info">
                            <img src="/images/${item.sanPham.imageURL}" 
                                 alt="${item.sanPham.tenSanPham}" 
                                 class="cart-img">
                            <div class="cart-text">
                                <p class="mb-0">${item.sanPham.tenSanPham}</p>
                            </div>
                        </div>
                    </td>

                    <td class="py-4">
                        <p class="mb-0 py-4">${item.sanPham.loaiSanPham.tenLoaiSanPham}</p>
                    </td>

                    <td class="py-4 price">
                        <p class="mb-0 py-4">${item.sanPham.gia.toLocaleString()} ₫</p>
                    </td>

                    <td class="py-4">
                        <div class="input-group quantity py-4" style="width: 100px;">
                            <div class="input-group-btn">
                                <button class="btn btn-sm btn-minus rounded-circle bg-light border" data-id="${item.idSanPham}">
                                    <i class="fa fa-minus"></i>
                                </button>
                            </div>

                            <input type="text"
                                   class="form-control form-control-sm text-center border-0 qty"
                                   value="${item.soLuongTrongGio}">

                            <div class="input-group-btn">
                                <button class="btn btn-sm btn-plus rounded-circle bg-light border" data-id="${item.idSanPham}">
                                    <i class="fa fa-plus"></i>
                                </button>
                            </div>
                        </div>
                    </td>

                    <td class="py-4 line-total">
                        <p class="mb-0 py-4">${total.toLocaleString()} ₫</p>
                    </td>

                    <td class="py-4">
                        <button class="btn btn-md rounded-circle bg-light border delete-item"
                                data-id="${item.idSanPham}">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"
                                 fill="currentColor" class="bi bi-trash3-fill" viewBox="0 0 16 16">
                                <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5"/>
                            </svg>
                        </button>
                    </td>
                `;
                tbody.appendChild(tr);
            });
            document.querySelectorAll(".btn-plus").forEach(btn => {
                btn.addEventListener("click", function () {
                    const productId = this.getAttribute("data-id");
                    const row = this.closest("tr");
                    const qtyInput = row.querySelector(".qty");

                    fetch(`/api/cart/plus?idSanPham=${productId}`, {
                        method: "PUT",
                        credentials: "include"
                    })
                        .then(res => res.json())
                        .then(data => {
                            qtyInput.value = data.soLuong;

                            const price = parseInt(
                                row.querySelector(".price").innerText.replace(/\D/g, "")
                            );

                            row.querySelector(".line-total p").innerText =
                                (price * data.soLuong).toLocaleString() + " ₫";

                            updateTotals();
                            updateCartBadge();
                        });
                });
            });

            document.querySelectorAll(".btn-minus").forEach(btn => {
                btn.addEventListener("click", function () {
                    const productId = this.getAttribute("data-id");
                    const row = this.closest("tr");
                    const qtyInput = row.querySelector(".qty");

                    fetch(`/api/cart/minus?idSanPham=${productId}`, {
                        method: "PUT",
                        credentials: "include"
                    })
                        .then(res => {
                            if (!res.ok) return;
                            return res.json();
                        })
                        .then(data => {
                            if (!data) return;

                            qtyInput.value = data.soLuong;

                            const price = parseInt(
                                row.querySelector(".price").innerText.replace(/\D/g, "")
                            );

                            row.querySelector(".line-total p").innerText =
                                (price * data.soLuong).toLocaleString() + " ₫";

                            updateTotals();
                            updateCartBadge();
                        });
                });
            });


            updateTotals();

            document.querySelectorAll(".cart-check").forEach(cb => {
                cb.addEventListener("change", updateTotals);
            });

            document.querySelectorAll(".qty").forEach(input => {
                input.addEventListener("change", function () {
                    const row = this.closest("tr");
                    const price = parseInt(row.querySelector(".price").innerText.replace(/\D/g, ""));
                    const qty = parseInt(this.value) || 1;
                    row.querySelector(".line-total p").innerText =
                        (price * qty).toLocaleString() + " ₫";
                    updateTotals();
                });
            });

            document.querySelectorAll(".delete-item").forEach(btn => {
                btn.addEventListener("click", function () {
                    const id = this.getAttribute("data-id");
                    const row = this.closest("tr");

                    Swal.fire({
                        title: "Bạn có chắc?",
                        text: "Sản phẩm sẽ được xóa khỏi giỏ hàng!",
                        icon: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#3085d6",
                        cancelButtonColor: "#d33",
                        cancelButtonText: "Hủy",
                        confirmButtonText: "Xóa"
                    }).then((result) => {
                        if (result.isConfirmed) {
                            fetch(`/api/cart/${id}`, {
                                method: "DELETE",
                                credentials: "include"
                            })
                                .then(res => res.json())
                                .then(json => {
                                    if (json.success) {
                                        row.remove();
                                        updateTotals();
                                        updateCartBadge();
                                        Swal.fire({
                                            title: "Đã xóa!",
                                            text: "Sản phẩm đã được xóa khỏi giỏ.",
                                            icon: "success",
                                            timer: 1200,
                                            showConfirmButton: false
                                        });
                                    }
                                });
                        }
                    });
                });
            });
        });

    function updateTotals() {
        let subtotal = 0;

        document.querySelectorAll("#cart-body tr").forEach(row => {
            const check = row.querySelector(".cart-check");
            if (!check.checked) return;

            const price = parseInt(row.querySelector(".price").innerText.replace(/\D/g, ""));
            const qty = parseInt(row.querySelector(".qty").value) || 1;
            subtotal += price * qty;
        });

        const shipping = subtotal > 0 ? 30000 : 0;
        const total = subtotal + shipping;

        document.querySelector(".subtotal-value").textContent = subtotal.toLocaleString() + " ₫";
        document.querySelector(".shipping-value").textContent = shipping.toLocaleString() + " ₫";
        document.querySelector(".total-value").textContent = total.toLocaleString() + " ₫";

        const amountInput = document.querySelector('input[name="Amount"]');
        if (amountInput) amountInput.value = total;

        const submitBtn = document.querySelector('form[action*="CreatePaymentMomo"] button[type="submit"]');
        if (submitBtn) submitBtn.disabled = total <= 0;
    }

    function updateCartBadge() {
        fetch("/api/cart/count", { credentials: "include" })
            .then(res => res.json())
            .then(json => {
                const badge = document.getElementById("cart-count-badge");
                const count = json.count || 0;
                if (count > 0) {
                    badge.style.display = "inline-block";
                    badge.textContent = count;
                } else {
                    badge.style.display = "none";
                }
            });
    }

});
