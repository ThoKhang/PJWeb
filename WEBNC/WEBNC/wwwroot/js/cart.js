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
                        <button class="btn btn-md rounded-circle bg-light border delete-item" data-id="${item.idSanPham}">
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
