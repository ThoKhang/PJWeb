$(document).ready(function () {

    const table = $('#tblRoles').DataTable({
        ajax: {
            url: '/api/admin/roles',
            dataSrc: ''
        },
        columns: [
            { data: 'name', className: 'text-center' },
            {
                data: 'id',
                orderable: false,
                className: 'text-center',
                render: id => `
                    <button class="btn btn-sm btn-danger btn-delete"
                            data-id="${id}">
                        <i class="fa-solid fa-trash"></i>
                    </button>
                `
            }
        ]
    });

    $('#btnAddRole').click(async function () {
        const roleName = $('#txtRoleName').val().trim();
        if (!roleName) return;

        const res = await fetch('/api/admin/roles', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(roleName)
        });

        if (res.ok) {
            Swal.fire('Thành công', 'Đã thêm role', 'success');
            $('#txtRoleName').val('');
            table.ajax.reload();
        } else {
            const err = await res.json();
            Swal.fire('Lỗi', err.message, 'error');
        }
    });

    $(document).on('click', '.btn-delete', async function () {
        const id = $(this).data('id');

        const confirm = await Swal.fire({
            title: 'Xóa role?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Xóa'
        });

        if (!confirm.isConfirmed) return;

        const res = await fetch(`/api/admin/roles/${id}`, {
            method: 'DELETE'
        });

        if (res.ok) {
            Swal.fire('Đã xóa', '', 'success');
            table.ajax.reload();
        } else {
            const err = await res.json();
            Swal.fire('Lỗi', err.message, 'error');
        }
    });
});
