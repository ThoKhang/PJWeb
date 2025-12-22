import { API_BASE_URL } from "../config/api.config-admin.js";

/* =========================
   HELPERS
========================= */
function normalizeImageUrl(img) {
    if (!img) return "";
    if (img.startsWith("/")) return img;
    return `/images/sanpham/${img}`;
}

function setCurrentImage(img) {
    const src = normalizeImageUrl(img);
    if (!src) {
        $("#previewImg").hide().attr("src", "");
        $("#txtNoImg").show();
        return;
    }
    $("#previewImg").attr("src", src).show();
    $("#txtNoImg").hide();
}

function previewSelectedFile(file) {
    if (!file) return;
    const url = URL.createObjectURL(file);
    $("#previewImg").attr("src", url).show();
    $("#txtNoImg").hide();
}

async function loadLoaiSanPham(selected) {
    const $sel = $("#idLoaiSanPham");
    $sel.prop("disabled", true).empty()
        .append(`<option value="">-- Đang tải loại sản phẩm... --</option>`);

    const res = await fetch(`${API_BASE_URL}/api/admin/loaisanpham`);
    if (!res.ok) throw new Error("Không tải được loại sản phẩm");

    const data = await res.json();
    $sel.empty().append(`<option value="">-- Chọn loại sản phẩm --</option>`);
    data.forEach(x => {
        $sel.append(`<option value="${x.idLoaiSanPham}">${x.tenLoaiSanPham}</option>`);
    });

    if (selected) $sel.val(selected);
    $sel.prop("disabled", false);
}

async function loadCongTy(selected) {
    const $sel = $("#idCongTy");
    $sel.prop("disabled", true).empty()
        .append(`<option value="">-- Đang tải công ty... --</option>`);

    const res = await fetch(`${API_BASE_URL}/api/admin/congty`);
    if (!res.ok) throw new Error("Không tải được công ty");

    const data = await res.json();
    $sel.empty().append(`<option value="">-- Chọn công ty --</option>`);
    data.forEach(x => {
        $sel.append(`<option value="${x.idCongTy}">${x.tenCongTy}</option>`);
    });

    if (selected) $sel.val(selected);
    $sel.prop("disabled", false);
}

/* =========================
   MAIN
========================= */
$(async function () {
    const id = ($("#idSanPham").val() || "").trim();

    try {
        // load dropdown trước
        await loadLoaiSanPham(null);
        await loadCongTy(null);

        if (id) {
            const res = await fetch(`${API_BASE_URL}/api/admin/sanpham/${id}`);
            if (!res.ok) {
                Swal.fire("Lỗi", "Không tìm thấy sản phẩm", "error");
                return;
            }

            const sp = await res.json();

            $("#tenSanPham").val(sp.tenSanPham ?? "");
            $("#gia").val(sp.gia ?? 0);
            $("#soLuong").val(sp.soLuongHienCon ?? 0);

            // nếu form có các field này thì set luôn
            if ($("#moTa").length) $("#moTa").val(sp.moTa ?? "");
            if ($("#thongSoSanPham").length) $("#thongSoSanPham").val(sp.thongSoSanPham ?? "");
            if ($("#imageLienQuan").length) $("#imageLienQuan").val(sp.imageLienQuan ?? "");

            await loadLoaiSanPham(sp.idLoaiSanPham);
            await loadCongTy(sp.idCongTy);

            // ảnh hiện tại
            setCurrentImage(sp.imageURL);
            $("#currentImageURL").val(sp.imageURL ?? "");
        } else {
            setCurrentImage("");
            $("#currentImageURL").val("");
        }
    } catch (e) {
        console.error(e);
        Swal.fire("Lỗi", e?.message || "Không thể tải dữ liệu", "error");
    }

    // preview ảnh khi chọn file
    $("#image").on("change", function () {
        const file = this.files?.[0];
        if (file) previewSelectedFile(file);
    });

    // submit
    $("#frmSanPham").on("submit", async function (e) {
        e.preventDefault();

        const idSanPham = ($("#idSanPham").val() || "").trim();
        const tenSanPham = ($("#tenSanPham").val() || "").trim();
        const gia = $("#gia").val();
        const soLuongHienCon = $("#soLuong").val();
        const idLoaiSanPham = $("#idLoaiSanPham").val();
        const idCongTy = $("#idCongTy").val();

        // optional fields (nếu không có input thì gửi chuỗi rỗng)
        const moTa = ($("#moTa").length ? $("#moTa").val() : "")?.toString().trim() || "";
        const thongSoSanPham = ($("#thongSoSanPham").length ? $("#thongSoSanPham").val() : "")?.toString().trim() || "";
        const imageLienQuan = ($("#imageLienQuan").length ? $("#imageLienQuan").val() : "")?.toString().trim() || "";

        if (!tenSanPham) {
            Swal.fire("Thiếu dữ liệu", "Vui lòng nhập tên sản phẩm", "warning");
            return;
        }
        if (!idLoaiSanPham) {
            Swal.fire("Thiếu dữ liệu", "Vui lòng chọn loại sản phẩm", "warning");
            return;
        }
        if (!idCongTy) {
            Swal.fire("Thiếu dữ liệu", "Vui lòng chọn công ty", "warning");
            return;
        }

        const formData = new FormData();

        // ✅ map đúng theo property của SanPham (đúng tên)
        formData.append("idSanPham", idSanPham);
        formData.append("tenSanPham", tenSanPham);
        formData.append("gia", gia ?? 0);
        formData.append("soLuongHienCon", soLuongHienCon ?? 0);
        formData.append("idLoaiSanPham", idLoaiSanPham);
        formData.append("idCongTy", idCongTy);

        // ✅ gửi đủ field để không bị 400 validation/DB not null
        formData.append("moTa", moTa);
        formData.append("thongSoSanPham", thongSoSanPham);
        formData.append("imageLienQuan", imageLienQuan);

        const file = $("#image")[0]?.files?.[0];

        if (file) {
            // Controller param là IFormFile? image => key phải là "image"
            formData.append("image", file);
            // imageURL sẽ do BE set sau khi upload
            formData.append("imageURL", "");
        } else {
            // không chọn file => giữ ảnh cũ khi update
            formData.append("imageURL", ($("#currentImageURL").val() || "").trim());
        }

        try {
            const res = await fetch(`${API_BASE_URL}/api/admin/sanpham/upsert`, {
                method: "POST",
                body: formData
            });

            if (!res.ok) {
                // đọc lỗi JSON validation nếu có
                const ct = res.headers.get("content-type") || "";
                if (ct.includes("application/json")) {
                    const err = await res.json();
                    console.error("API Error:", err);
                    Swal.fire("Lỗi", JSON.stringify(err.errors || err, null, 2), "error");
                } else {
                    const txt = await res.text().catch(() => "");
                    Swal.fire("Lỗi", txt || "Không thể lưu sản phẩm", "error");
                }
                return;
            }

            Swal.fire({
                icon: "success",
                title: "Lưu thành công",
                timer: 1200,
                showConfirmButton: false
            }).then(() => {
                window.location.href = "/Admin/SanPham";
            });
        } catch (err) {
            console.error(err);
            Swal.fire("Lỗi", "Không thể kết nối server", "error");
        }
    });
});
