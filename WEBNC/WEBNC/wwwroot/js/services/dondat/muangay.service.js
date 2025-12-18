export function handleBuyNow(id, quantity) {

    const executeBuyNow = async (phone = null, address = null) => {
        try {
            const payload = {
                soLuong: quantity,
                sdt: phone,
                diaChi: address
            };

            const res = await fetch(
                `https://localhost:7047/api/customer/products/${id}`, {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (res.status === 401) {
                const returnUrl =
                    `/Customer/Home/Details?id=${encodeURIComponent(id)}`;
                window.location.href =
                    `/Identity/Account/Login?returnUrl=${encodeURIComponent(returnUrl)}`;
                return;
            }

            const data = await res.json();

            if (data.success === false && data.requiresInfo === true) {

                let formHtml = '';
                if (data.missingSdt) {
                    formHtml += `
                        <div style="margin-bottom: 15px; text-align: left;">
                            <label>Số điện thoại *</label>
                            <input id="swal-input-sdt" class="swal2-input">
                        </div>`;
                }
                if (data.missingAddress) {
                    formHtml += `
                        <div style="text-align: left;">
                            <label>Địa chỉ *</label>
                            <input id="swal-input-diachi" class="swal2-input">
                        </div>`;
                }

                const { value } = await Swal.fire({
                    title: 'Thông tin giao hàng',
                    html: formHtml,
                    showCancelButton: true,
                    preConfirm: () => {
                        return {
                            sdt: data.missingSdt
                                ? document.getElementById('swal-input-sdt').value
                                : phone,
                            diaChi: data.missingAddress
                                ? document.getElementById('swal-input-diachi').value
                                : address
                        };
                    }
                });

                if (value) {
                    executeBuyNow(value.sdt, value.diaChi);
                }
            }
            else if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Đặt hàng thành công!',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => {
                    window.location.href = data.redirectUrl;
                });
            }
            else {
                Swal.fire("Lỗi", data.message || "Đã có lỗi xảy ra", "error");
            }
        }
        catch (error) {
            console.error("Lỗi mua ngay:", error);
        }
    };

    executeBuyNow();
}
