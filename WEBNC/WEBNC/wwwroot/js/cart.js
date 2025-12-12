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
                   <th scope="row">
                        <div class="cart-item-info">
                            <img src="/images/${item.sanPham.imageURL}" 
                                 alt="${item.sanPham.tenSanPham}" 
                                 class="cart-img">

                            <div class="cart-text">
                                <p class="mb-0">${item.sanPham.tenSanPham}</p>
                            </div>
                        </div>
                    </th>

                    <td class="py-4">
                       <p class="mb-0 py-4">${item.sanPham.loaiSanPham.tenLoaiSanPham}</p>
                    </td>

                    <td class="py-4">
                        <p class="mb-0 py-4">${item.sanPham.gia.toLocaleString()} ₫</p>
                    </td>

                    <td class="py-4">
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

                    <td class="py-4">
                        <p class="mb-0 py-4">${total.toLocaleString()} ₫</p>
                    </td>

                    <td class="py-4">
                        <button class="btn btn-md rounded-circle bg-light border delete-item" data-id="${item.idSanPham}">
                           <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-trash3-fill" viewBox="0 0 16 16">
                              <path d="M11 1.5v1h3.5a.5.5 0 0 1 0 1h-.538l-.853 10.66A2 2 0 0 1 11.115 16h-6.23a2 2 0 0 1-1.994-1.84L2.038 3.5H1.5a.5.5 0 0 1 0-1H5v-1A1.5 1.5 0 0 1 6.5 0h3A1.5 1.5 0 0 1 11 1.5m-5 0v1h4v-1a.5.5 0 0 0-.5-.5h-3a.5.5 0 0 0-.5.5M4.5 5.029l.5 8.5a.5.5 0 1 0 .998-.06l-.5-8.5a.5.5 0 1 0-.998.06m6.53-.528a.5.5 0 0 0-.528.47l-.5 8.5a.5.5 0 0 0 .998.058l.5-8.5a.5.5 0 0 0-.47-.528M8 4.5a.5.5 0 0 0-.5.5v8.5a.5.5 0 0 0 1 0V5a.5.5 0 0 0-.5-.5"/>
                            </svg>
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

            const amountInput = document.querySelector('input[name="Amount"]');
            if (amountInput) {
                amountInput.value = totalAll;
            }

            const submitBtn = document.querySelector('form[action*="CreatePaymentMomo"] button[type="submit"]');
            if (submitBtn) {
                submitBtn.disabled = totalAll <= 0;
            }

            // Gán sự kiện xoá sản phẩm
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
                                        // Xóa hàng trong giao diện
                                        row.remove();
                                        // Tính lại tổng
                                        updateTotals();
                                        // Cập nhật badge
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
            const price = parseInt(row.children[2].innerText.replace(/\D/g, ""));
            const input = row.querySelector("input");
            const qty = parseInt(input.value) || 1;
            subtotal += price * qty;
        });

        const shipping = subtotal > 0 ? 30000 : 0;
        const total = subtotal + shipping;

        document.querySelector(".subtotal-value").textContent = subtotal.toLocaleString() + " ₫";
        document.querySelector(".shipping-value").textContent = shipping.toLocaleString() + " ₫";
        document.querySelector(".total-value").textContent = total.toLocaleString() + " ₫";

        const amountInput = document.querySelector('input[name="Amount"]');
        if (amountInput) amountInput.value = total;
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
