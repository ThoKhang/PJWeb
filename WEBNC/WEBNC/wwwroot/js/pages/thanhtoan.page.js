// wwwroot/js/pages/thanhtoan.page.js

$(function () {
    const $tbl = $("#tblThanhToan");

    // Nếu table không tồn tại thì thôi (tránh lỗi khi nhúng nhầm page)
    if ($tbl.length === 0) return;

    const table = $tbl.DataTable({
        processing: true,
        serverSide: false,
        autoWidth: false,
        responsive: true,
        order: [[0, "desc"]],
        ajax: {
            url: "/api/admin/thanhtoan",
            type: "GET",
            dataSrc: function (json) {
                // Hỗ trợ 2 kiểu response:
                // 1) API trả về root array: [ ... ]
                // 2) API trả về object: { data: [ ... ] }
                if (Array.isArray(json)) return json;
                if (json && Array.isArray(json.data)) return json.data;
                return [];
            }
        },
        columns: [
            // 1) Mã TT
            { data: "idThanhToan" },

            // 2) Mã đơn
            {
                data: "donDatHang",
                render: function (d) {
                    return d && d.idDonDat ? d.idDonDat : "---";
                }
            },

            // 3) Phương thức
            {
                data: "phuongThuc",
                render: function (p) {
                    if (!p) return '<span class="badge bg-secondary">Không rõ</span>';
                    return '<span class="badge bg-info">' + p + "</span>";
                }
            },

            // 4) Số tiền
            {
                data: "soTien",
                className: "text-end",
                render: function (v) {
                    if (v == null) return "---";
                    return Number(v).toLocaleString("vi-VN") + " ₫";
                }
            },

            // 5) Trạng thái
            {
                data: "daThanhToan",
                className: "text-center",
                render: function (v, type, row) {
                    if (v) {
                        return '<span class="badge bg-success">Đã thanh toán</span>';
                    }
                    // Cho phép click vào trạng thái để mark-paid
                    return (
                        '<span class="badge bg-warning trang-thai-tt" style="cursor:pointer" ' +
                        'data-id="' + row.idThanhToan + '">' +
                        "Chưa thanh toán</span>"
                    );
                }
            },

            // 6) Ngày thanh toán
            {
                data: "ngayThanhToan",
                render: function (d) {
                    if (!d) return "-";
                    const dt = new Date(d);
                    if (isNaN(dt.getTime())) return d;
                    return dt.toLocaleString("vi-VN");
                }
            },

            // 7) Mã giao dịch
            { data: "maGiaoDich" },

            // 8) Thao tác
            {
                data: null,
                className: "text-center",
                orderable: false,
                searchable: false,
                render: function (_, __, row) {
                    const id = row.idThanhToan;
                    const detailUrl = "/Admin/ThanhToan/Details/" + id;

                    const markPaidBtn = row.daThanhToan
                        ? ""
                        : `
              <button class="btn btn-sm btn-success btn-mark-paid me-1" data-id="${id}">
                <i class="fa fa-check"></i>
              </button>
            `;

                    return `
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

    // Làm mới
    $("#btnRefreshThanhToan").on("click", function () {
        table.ajax.reload(null, false);
    });

    // ===== ĐÁNH DẤU ĐÃ THANH TOÁN (nút xanh) =====
    $tbl.on("click", ".btn-mark-paid", function () {
        const id = $(this).data("id");
        const row = table.row($(this).closest("tr")).data();
        if (!row || row.daThanhToan) return;

        confirmMarkPaid(id, row);
    });

    // ===== ĐÁNH DẤU ĐÃ THANH TOÁN (click badge "Chưa thanh toán") =====
    $tbl.on("click", ".trang-thai-tt", function () {
        const id = $(this).data("id");
        const row = table.row($(this).closest("tr")).data();
        if (!row || row.daThanhToan) return;

        confirmMarkPaid(id, row);
    });

    function confirmMarkPaid(id, row) {
        Swal.fire({
            title: "Xác nhận thanh toán?",
            html:
                "Đơn hàng: <b>" + (row.donDatHang ? row.donDatHang.idDonDat : "") + "</b><br>" +
                "Số tiền: <b class=\"text-success\">" + Number(row.soTien || 0).toLocaleString("vi-VN") + " ₫</b>",
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
    }

    // ===== XOÁ THANH TOÁN =====
    $tbl.on("click", ".btn-delete", function () {
        const id = $(this).data("id");

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
