// wwwroot/js/pages/thanhtoan.page.js

$(function () {

    var table = $("#tblThanhToan").DataTable({
        processing: true,
        serverSide: false,
        autoWidth: false,
        responsive: true,
        order: [[0, "desc"]],
        ajax: {
            url: "/api/admin/thanhtoan",
            type: "GET",
            dataSrc: ""
        },
        columns: [
            // 1️⃣ Mã thanh toán
            { data: "idThanhToan" },

            // 2️⃣ Mã đơn đặt hàng
            {
                data: "donDatHang",
                render: function (d) {
                    return d && d.idDonDat ? d.idDonDat : "---";
                }
            },

            // 3️⃣ Phương thức
            {
                data: "phuongThuc",
                render: function (p) {
                    if (!p)
                        return '<span class="badge bg-secondary">Không rõ</span>';
                    return '<span class="badge bg-info">' + p + '</span>';
                }
            },

            // 4️⃣ Số tiền
            {
                data: "soTien",
                className: "text-end",
                render: function (v) {
                    if (v == null) return "---";
                    return Number(v).toLocaleString("vi-VN") + " ₫";
                }
            },

            // 5️⃣ Trạng thái
            {
                data: "daThanhToan",
                className: "text-center",
                render: function (v) {
                    if (v) {
                        return '<span class="badge bg-success">Đã thanh toán</span>';
                    }
                    return '<span class="badge bg-warning">Chưa thanh toán</span>';
                }
            },

            // 6️⃣ Ngày thanh toán
            {
                data: "ngayThanhToan",
                render: function (d) {
                    if (!d) return "-";
                    var dt = new Date(d);
                    if (isNaN(dt.getTime())) return d;
                    return dt.toLocaleString("vi-VN");
                }
            },

            // 7️⃣ Mã giao dịch
            { data: "maGiaoDich" },

            // 8️⃣ Thao tác
            {
                data: "idThanhToan",
                className: "text-center",
                orderable: false,
                searchable: false,
                render: function (id, type, row) {
                    var detailUrl = "/Admin/ThanhToan/Details/" + id;

                    var markPaidBtn = row.daThanhToan ? "" :
                        '<button class="btn btn-sm btn-success btn-mark-paid me-1" data-id="' + id + '">' +
                        '<i class="fa fa-check"></i>' +
                        '</button>';

                    return '' +
                        '<a href="' + detailUrl + '" class="btn btn-sm btn-info me-1">' +
                        '<i class="fa fa-info-circle"></i>' +
                        '</a>' +
                        markPaidBtn +
                        '<button class="btn btn-sm btn-danger btn-delete" data-id="' + id + '">' +
                        '<i class="fa fa-trash"></i>' +
                        '</button>';
                }
            }
        ],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        }
    });

    // 🔄 Làm mới (reload dữ liệu bảng, không reload trang)
    $("#btnRefreshThanhToan").on("click", function () {
        table.ajax.reload(null, false);
    });

    // ✅ Đánh dấu đã thanh toán (nút màu xanh)
    $("#tblThanhToan").on("click", ".btn-mark-paid", function () {
        var id = $(this).data("id");
        var row = table.row($(this).closest("tr")).data();

        Swal.fire({
            title: "Xác nhận thanh toán?",
            html:
                "Đơn hàng: <b>" + (row.donDatHang ? row.donDatHang.idDonDat : "") + "</b><br>" +
                "Số tiền: <b class=\"text-success\">" +
                Number(row.soTien).toLocaleString("vi-VN") + " ₫</b>",
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Huỷ"
        }).then(function (result) {
            if (!result.isConfirmed) return;

            $.ajax({
                url: "/api/admin/thanhtoan/" + id + "/mark-paid",
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

    // 🗑 Xoá thanh toán
    $("#tblThanhToan").on("click", ".btn-delete", function () {
        var id = $(this).data("id");

        Swal.fire({
            title: "Xoá thanh toán?",
            text: "Hành động này không thể hoàn tác!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Xoá",
            cancelButtonText: "Huỷ",
            confirmButtonColor: "#d33"
        }).then(function (result) {
            if (!result.isConfirmed) return;

            $.ajax({
                url: "/api/admin/thanhtoan/" + id,
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
