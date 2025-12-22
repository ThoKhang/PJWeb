// wwwroot/js/pages/thanhtoan.page.js

$(document).ready(function () {
    const table = $("#tblThanhToan").DataTable({
        processing: true,
        serverSide: false,
        ajax: {
            url: "/api/admin/thanhtoan",
            type: "GET",
            dataSrc: ""
        },
        columns: [
            { data: "idThanhToan" },
            {
                data: "donDatHang",
                render: function (data) {
                    // chỉnh lại property cho đúng với model DonDatHang của bạn
                    // ví dụ: data?.idDonDat hoặc data?.maDon
                    return data && data.idDonDat ? data.idDonDat : "";
                }
            },
            { data: "phuongThuc" },
            {
                data: "soTien",
                render: function (data) {
                    if (data == null) return "";
                    return Number(data).toLocaleString("vi-VN") + " ₫";
                }
            },
            {
                data: "daThanhToan",
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Đã thanh toán</span>'
                        : '<span class="badge bg-secondary">Chưa thanh toán</span>';
                }
            },
            {
                data: "ngayThanhToan",
                render: function (data) {
                    if (!data) return "-";
                    const dt = new Date(data);
                    if (isNaN(dt.getTime())) return data;
                    return dt.toLocaleString("vi-VN");
                }
            },
            { data: "maGiaoDich" },
            {
                data: "idThanhToan",
                orderable: false,
                searchable: false,
                render: function (id, type, row) {
                    const detailUrl = `/Admin/ThanhToan/Details/${id}`;

                    const markPaidBtn = row.daThanhToan
                        ? ""
                        : `<button class="btn btn-sm btn-success btn-mark-paid" data-id="${id}">
                               <i class="fa fa-check"></i>
                           </button>`;

                    return `
                        <a href="${detailUrl}" class="btn btn-sm btn-info me-1">
                            <i class="fa fa-info-circle"></i>
                        </a>
                        ${markPaidBtn}
                        <button class="btn btn-sm btn-danger btn-delete" data-id="${id}">
                            <i class="fa fa-trash"></i>
                        </button>
                    `;
                }
            }
        ],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        }
    });

    // Xoá thanh toán
    $("#tblThanhToan").on("click", ".btn-delete", function () {
        const id = $(this).data("id");

        Swal.fire({
            title: "Xoá thanh toán?",
            text: "Bạn sẽ không thể khôi phục lại bản ghi này!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Xoá",
            cancelButtonText: "Huỷ"
        }).then(result => {
            if (!result.isConfirmed) return;

            $.ajax({
                url: `/api/admin/thanhtoan/${id}`,
                type: "DELETE",
                success: function () {
                    Swal.fire("Đã xoá!", "Thanh toán đã được xoá.", "success");
                    table.ajax.reload(null, false);
                },
                error: function (xhr) {
                    const msg =
                        (xhr.responseJSON && xhr.responseJSON.message) ||
                        "Không thể xoá thanh toán này.";
                    Swal.fire("Lỗi", msg, "error");
                }
            });
        });
    });

    // Đánh dấu đã thanh toán
    $("#tblThanhToan").on("click", ".btn-mark-paid", function () {
        const id = $(this).data("id");

        Swal.fire({
            title: "Đánh dấu đã thanh toán?",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Đồng ý",
            cancelButtonText: "Huỷ"
        }).then(result => {
            if (!result.isConfirmed) return;

            $.ajax({
                url: `/api/admin/thanhtoan/${id}/mark-paid`,
                type: "PATCH",
                success: function () {
                    Swal.fire("Thành công", "Đã cập nhật trạng thái thanh toán.", "success");
                    table.ajax.reload(null, false);
                },
                error: function (xhr) {
                    const msg =
                        (xhr.responseJSON && xhr.responseJSON.message) ||
                        "Không thể cập nhật thanh toán.";
                    Swal.fire("Lỗi", msg, "error");
                }
            });
        });
    });
});
