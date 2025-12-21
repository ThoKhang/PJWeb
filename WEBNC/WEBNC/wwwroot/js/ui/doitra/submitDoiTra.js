// File: /js/ui/doitra/submitDoiTra.js
import { apiPostYeuCauDoiTra } from "../../api/doitra/doitra.api.js";

export async function submitYeuCauDoiTra(idDonDat) {
    // 1. Lấy giá trị từ Input (Giả sử ID input là txtLyDo-{idDonDat})
    const txtLyDo = document.getElementById(`txtLyDo-${idDonDat}`);
    const lyDoVal = txtLyDo ? txtLyDo.value.trim() : "";

    // 2. Validate phía Client
    if (!lyDoVal) {
        Swal.fire({
            icon: 'warning',
            title: 'Chưa nhập lý do',
            text: 'Vui lòng cho biết lý do bạn muốn đổi trả sản phẩm này.'
        });
        return;
    }

    if (lyDoVal.length > 300) {
        Swal.fire("Lý do quá dài (tối đa 300 ký tự)!");
        return;
    }

    // 3. Chuẩn bị FormData
    let formData = new FormData();
    formData.append("idDonDat", idDonDat);
    formData.append("lyDo", lyDoVal);

    // UX: Hiển thị loading (nếu có nút bấm)
    const btnSubmit = document.getElementById(`btnSubmit-${idDonDat}`);
    if (btnSubmit) {
        btnSubmit.disabled = true;
        btnSubmit.innerText = "Đang gửi...";
    }

    try {
        // 4. Gọi API
        const res = await apiPostYeuCauDoiTra(formData);

        if (res.ok) {
            Swal.fire({
                toast: true,
                position: "top-end",
                icon: "success",
                title: "Đã gửi yêu cầu đổi trả!",
                showConfirmButton: false,
                timer: 3000
            }).then(() => {
                // Chuyển hướng sang trang Lịch sử
                window.location.href = "/Customer/YeuCauDoiTra/History";
            });;

        } else {
            // Đọc lỗi từ server trả về
            const errorData = await res.json().catch(() => ({}));
            Swal.fire("Lỗi", errorData.message || "Không thể gửi yêu cầu.", "error");
        }
    } catch (error) {
        console.log("LỖI CỤ THỂ LÀ:", error);
        Swal.fire("Lỗi", "Đã xảy ra lỗi kết nối.", "error");
    } finally {
        // Mở lại nút
        if (btnSubmit) {
            btnSubmit.disabled = false;
            btnSubmit.innerText = "Xác nhận đổi trả";
        }
    }
}