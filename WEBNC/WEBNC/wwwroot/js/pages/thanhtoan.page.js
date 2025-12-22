// wwwroot/js/pages/thanhtoan.page.js
$(document).ready(function () {

    const table = $("#tblThanhToan").DataTable({
        processing: true,
        serverSide: false,
        autoWidth: false,
        responsive: true,
        order: [[0, "desc"]],
        ajax: {
            url: "/api/admin/thanhtoan",
            dataSrc: ""
        },
        columns: [
            // Mã thanh toán
            { data: "idThanhToan" },

            // Mã đơn hàng
            {
                data: "donDatHang",
                render: d => d?.idDonDat ?? "---"
            },

            // Phương thức
            {
                data: "phuongThuc",
                render: p => `<span class="badge bg-info">${p}</span>`
            },

            // Số tiền
            {
                data: "soTien",
                className: "text-end",
                render: v =>
                    v ? `${Number(v).toLocaleString("vi-VN")} ₫` : "---"
            },

            // Trạng thái thanh toán
            {
                data: "daThanhToan",
                className: "text-center",
                render: v => v
                    ? `<span class="badge bg-success">Đã thanh toán</span>`
                    : `<span class="badge bg-warning trang-thai-tt" style="cursor:pointer">
                            Chưa thanh toán
                       </span>`
            },

            // Ngày thanh toán
            {
                data: "ngayThanhToan",
                render: d => d ? new Date(d).toLocaleString("vi-VN") : "-"
            },

            // Mã giao dịch
            { data: "maGiaoDich" },

            // Thao tác
            {
                data: "idThanhToan",
                className: "text-center",
                orderable: false,
                searchable: false,
                render: function (id, type, row) {

                    const detailUrl = `/Admin/ThanhToan/Details/${id}`;

                    const markPaidBtn = row.daThanhToan
                        ? ""
                        : `
                            <button class="btn btn-sm btn-success btn-mark-paid me-1"
                                    data-id="${id}">
                                <i class="fa fa-check"></i>
                            </button>
                          `;

                    return `
                        <a href="${detailUrl}" class="btn btn-sm btn-info me-1">
                            <i class="fa fa-eye"></i>
                        </a>
                        ${markPaidBtn}
                        <button class="btn btn-sm btn-danger btn-delete"
                                data-id="${id}">
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

    // ===== ĐÁNH DẤU ĐÃ THANH TOÁN =====
    $("#tblThanhToan").on("click", ".trang-thai-tt", function () {

        const row = table.row($(this).closest("tr")).data();
        if (row.daThanhToan) return;

        Swal.fire({
            title: "Xác nhận thanh toán?",
            html: `
                Đơn hàng: <b>${row.donDatHang?.idDonDat ?? ""}</b><br>
                Số tiền: <b class="text-success">
                    ${Number(row.soTien).toLocaleString("vi-VN")} ₫
                </b>
            `,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Huỷ"
        }).then(result => {

            if (!result.isConfirmed) return;

            $.ajax({
                url: `/api/admin/thanhtoan/${row.idThanhToan}/mark-paid`,
                type: "PATCH",
                success: function () {
                    Swal.fire({
                        icon: "success",
                        title: "Đã cập nhật!",
                        timer: 1200,
                        showConfirmButton: false
                    });
                    table.ajax.reload(null, false);
                },
                error: function () {
                    Swal.fire("Lỗi", "Không thể cập nhật thanh toán!", "error");
                }
            });
        });
    });

    // ===== XOÁ THANH TOÁN =====
    $("#tblThanhToan").on("click", ".btn-delete", function () {

        const id = $(this).data("id");

        Swal.fire({
            title: "Xoá thanh toán?",
            text: "Hành động này không thể hoàn tác!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Xoá",
            cancelButtonText: "Huỷ",
            confirmButtonColor: "#d33"
        }).then(result => {

            if (!result.isConfirmed) return;

            $.ajax({
                url: `/api/admin/thanhtoan/${id}`,
                type: "DELETE",
                success: function () {
                    Swal.fire({
                        icon: "success",
                        title: "Đã xoá!",
                        timer: 1200,
                        showConfirmButton: false
                    });
                    table.ajax.reload(null, false);
                },
                error: function () {
                    Swal.fire("Lỗi", "Không thể xoá thanh toán!", "error");
                }
            });
        });
    });
});
