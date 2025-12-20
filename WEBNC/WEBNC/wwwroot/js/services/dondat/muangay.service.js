
export function handleBuyNow(id, quantity = 1) {
    let phuongXaList = []; // Cache danh sách phường/xã

    // Load danh sách phường/xã từ API (chỉ load 1 lần)
    const loadPhuongXaList = async () => {
        if (phuongXaList.length > 0) return; // Đã load rồi

        try {
            const res = await fetch('https://localhost:7047/api/DonDatHang/get-phuongxa', {
                credentials: "include"
            });

            if (res.ok) {
                phuongXaList = await res.json();
            } else {
                console.error('Không thể tải danh sách phường/xã');
                phuongXaList = [];
            }
        } catch (err) {
            console.error('Lỗi kết nối API phường/xã:', err);
            phuongXaList = [];
        }
    };

    const executeBuyNow = async (extraData = {}) => {
        try {
            const payload = {
                soLuong: quantity,
                ...extraData
            };

            const res = await fetch(`https://localhost:7047/api/customer/products/${id}`, {
                method: "POST",
                credentials: "include",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(payload)
            });

            if (res.status === 401) {
                window.location.href = `/Identity/Account/Login?returnUrl=${encodeURIComponent(window.location.href)}`;
                return;
            }

            const data = await res.json();

            // Nếu thiếu thông tin → hiện modal nhập
            if (data.success === false && data.requiresInfo === true) {
                // Nếu thiếu địa chỉ → load danh sách phường/xã trước
                if (data.missingAddress) {
                    await loadPhuongXaList();
                }

                let html = '<div style="text-align: left; padding: 10px;">';

                if (data.missingPhone) {
                    html += `
                        <div class="mb-3">
                            <label class="form-label fw-bold">Số điện thoại <span class="text-danger">*</span></label>
                            <input type="tel" id="modal-phone" class="form-control" placeholder="Ví dụ: 0901234567" maxlength="15">
                        </div>`;
                }

                if (data.missingAddress) {
                    html += `
                        <div class="mb-3">
                            <label class="form-label fw-bold">Số nhà, tên đường <span class="text-danger">*</span></label>
                            <input type="text" id="modal-sonha" class="form-control" placeholder="Ví dụ: 123 Nguyễn Trãi">
                        </div>
                        <div class="mb-3">
                            <label class="form-label fw-bold">Phường/Xã <span class="text-danger">*</span></label>
                            <select id="modal-phuongxa" class="form-select">
                                <option value="">-- Chọn phường/xã --</option>
                                ${phuongXaList.map(item => `<option value="${item.id}">${item.text}</option>`).join('')}
                            </select>
                        </div>`;
                }

                html += '</div>';

                const { value: formValues } = await Swal.fire({
                    title: 'Vui lòng nhập thông tin giao hàng',
                    html: html,
                    width: '600px',
                    focusConfirm: false,
                    showCancelButton: true,
                    confirmButtonText: 'Đặt hàng',
                    cancelButtonText: 'Hủy',
                    preConfirm: () => {
                        const phone = data.missingPhone ? document.getElementById('modal-phone')?.value.trim() : null;
                        const sonha = data.missingAddress ? document.getElementById('modal-sonha')?.value.trim() : null;
                        const phuongxa = data.missingAddress ? document.getElementById('modal-phuongxa')?.value : null;

                        if (data.missingPhone && !phone) {
                            Swal.showValidationMessage('Vui lòng nhập số điện thoại');
                            return false;
                        }
                        if (data.missingAddress && !sonha) {
                            Swal.showValidationMessage('Vui lòng nhập số nhà');
                            return false;
                        }
                        if (data.missingAddress && !phuongxa) {
                            Swal.showValidationMessage('Vui lòng chọn phường/xã');
                            return false;
                        }

                        return { sdt: phone, soNha: sonha, idPhuongXa: phuongxa };
                    }
                });

                if (formValues) {
                    // Gọi lại với thông tin mới → backend sẽ lưu + tạo đơn
                    executeBuyNow(formValues);
                }
                return;
            }

            // Thành công
            if (data.success) {
                Swal.fire({
                    icon: 'success',
                    title: 'Đặt hàng thành công!',
                    text: data.message || 'Đơn hàng đã được tạo.',
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    window.location.href = data.redirectUrl || "/Customer/DonDatHang/Index";
                });
            } else {
                Swal.fire('Lỗi', data.message || 'Đã có lỗi xảy ra', 'error');
            }
        } catch (error) {
            console.error('Lỗi khi đặt hàng:', error);
            Swal.fire('Lỗi kết nối', 'Không thể kết nối đến server. Vui lòng thử lại.', 'error');
        }
    };

    // Bắt đầu thực hiện
    executeBuyNow();
}