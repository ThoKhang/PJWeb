// wwwroot/js/pages/thanhtoan.page.js
$(document).ready(function () {

    const table = $("#tblThanhToan").DataTable({
        ajax: {
            url: "/api/admin/thanhtoan",
            dataSrc: ""
        },
        columns: [
            { data: "idThanhToan" },

            {
                data: "donDatHang",
                render: d => d?.idDonDat ?? "---"
            },

            {
                data: "phuongThuc",
                render: p => `<span class="badge bg-info">${p}</span>`
            },
            {
                data: "soTien",
                className: "text-end",
                render: v =>
                    v ? `${Number(v).toLocaleString("vi-VN")} ₫` : "---"
            },

            {
                data: "daThanhToan",
                className: "text-center",
                render: v => v
                    ? `<span class="badge bg-success trang-thai-tt" data-status="paid">Đã thanh toán</span>`
                    : `<span class="badge bg-warning trang-thai-tt" data-status="unpaid" style="cursor:pointer">
                            Chưa thanh toán
                       </span>`
            },
            {
                data: "ngayThanhToan",
                render: d =>
                    d ? new Date(d).toLocaleString("vi-VN") : "-"
            },

            { data: "maGiaoDich" },

            {
                data: "idThanhToan",
                className: "text-center",
                orderable: false,
                render: id => `
                    <a href="/Admin/ThanhToan/Details/${id}"
                       class="btn btn-sm btn-info me-1">
                        <i class="fa fa-eye"></i>
                    </a>
                    <button class="btn btn-sm btn-danger btn-delete"
                            data-id="${id}">
                        <i class="fa fa-trash"></i>
                    </button>
                `
            }
        ],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        }
    });

       //ĐÁNH DẤU ĐÃ THANH TOÁN
    $("#tblThanhToan").on("click", ".trang-thai-tt", function () {

        const badge = $(this);
        const row = table.row(badge.closest("tr")).data();

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
            cancelButtonText: "Huỷ",
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33"
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

       //XOÁ THANH TOÁN
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
