// File: /wwwroot/js/ui/doitra/history.js
import { apiGetLichSuDoiTra, apiDeleteYeuCauDoiTra } from "../../api/doitra/doitra.api.js";
// Hàm khởi chạy chính
export async function initHistoryPage() {
    const loadingSpinner = document.getElementById('loadingSpinner');
    const tableContainer = document.getElementById('requestTableContainer');
    const noDataMessage = document.getElementById('noDataMessage');

    try {
        const response = await apiGetLichSuDoiTra();

        if (response.status === 401) {
            window.location.href = "/Identity/Account/Login";
            return;
        }

        const res = await response.json();
        const data = res && res.data ? res.data : [];

        // Tắt loading
        if (loadingSpinner) loadingSpinner.style.display = 'none';

        if (data.length === 0) {
            if (noDataMessage) noDataMessage.style.display = 'block';
            if (tableContainer) tableContainer.style.display = 'none';
            return;
        }

        if (tableContainer) tableContainer.style.display = 'block';
        renderReturnTable(data);

    } catch (error) {
        console.error('Error:', error);
        if (loadingSpinner) {
            loadingSpinner.innerHTML = '<p class="text-danger"><i class="fas fa-exclamation-triangle"></i> Lỗi kết nối server.</p>';
        }
    }
}

// Hàm render bảng (Private function trong module này)
function renderReturnTable(data) {
    const tbody = document.getElementById('tblYeuCauBody');
    if (!tbody) return;

    let html = '';

    data.forEach(item => {
        // Xử lý ngày tháng...
        let dateString = "---";
        if (item.ngayTao) {
            const dateObj = new Date(item.ngayTao);
            dateString = dateObj.toLocaleDateString('vi-VN') + ' ' + dateObj.toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' });
        }

        // Xử lý trạng thái và NÚT HỦY
        let statusText = item.trangThai || "Chờ xử lý";
        let badgeClass = "bg-secondary";
        let actionButton = ""; // Biến chứa nút hủy

        if (statusText === 'ChoDuyet' || statusText === 'Chờ duyệt') {
            badgeClass = "bg-warning text-dark";
            statusText = "Đang chờ duyệt";

            // CHỈ HIỆN NÚT HỦY KHI ĐANG CHỜ DUYỆT
            actionButton = `
                <button class="btn btn-outline-danger btn-sm ms-2" onclick="window.handleCancelRequest(${item.idYeuCau})">
                    <i class="fas fa-trash-alt"></i> Hủy
                </button>
            `;
        }
        else if (statusText === 'DaDuyet' || statusText === 'Đã duyệt') {
            badgeClass = "bg-success";
            statusText = "Đã chấp nhận";
        }
        else if (statusText === 'TuChoi' || statusText === 'Từ chối') {
            badgeClass = "bg-danger";
            statusText = "Bị từ chối";
        }

        html += `
            <tr>
                <td class="fw-bold">#${item.idDonDat}</td>
                <td>${dateString}</td>
                <td>
                    <div class="text-truncate" style="max-width: 300px;" title="${item.lyDo}">
                        ${item.lyDo}
                    </div>
                </td>
                <td class="text-center">
                    <span class="badge ${badgeClass} p-2">
                        ${statusText}
                    </span>
                    <div class="mt-1">${actionButton}</div>
                </td>
            </tr>
        `;
    });

    tbody.innerHTML = html;
}

// --- HÀM XỬ LÝ HỦY (Gán vào window để HTML gọi được) ---
window.handleCancelRequest = async (idYeuCau) => {
    const result = await Swal.fire({
        title: 'Hủy yêu cầu?',
        text: "Bạn có chắc muốn hủy yêu cầu đổi trả này không?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Đồng ý hủy',
        cancelButtonText: 'Thôi'
    });

    if (result.isConfirmed) {
        try {
            // Gọi API
            const res = await apiDeleteYeuCauDoiTra(idYeuCau);

            if (res.ok) {
                Swal.fire('Đã hủy!', 'Yêu cầu của bạn đã được hủy.', 'success')
                    .then(() => {
                        // Tải lại trang để cập nhật danh sách
                        initHistoryPage();
                    });
            } else {
                const err = await res.json().catch(() => ({}));
                Swal.fire('Lỗi', err.message || "Không thể hủy.", 'error');
            }
        } catch (error) {
            Swal.fire('Lỗi', 'Lỗi kết nối server.', 'error');
        }
    }
};