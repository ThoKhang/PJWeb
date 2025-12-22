(function () {
    console.log("[admin.js] loaded");

    const sidebar = document.getElementById('adminSidebar');
    const main = document.getElementById('adminMain');
    const overlay = document.getElementById('adminOverlay');
    const btnToggle = document.getElementById('btnSidebarToggle');
    const btnClose = document.getElementById('btnSidebarClose');

    if (!sidebar || !main || !overlay || !btnToggle || !btnClose) {
        console.log("[admin.js] missing elements", { sidebar, main, overlay, btnToggle, btnClose });
        return;
    }

    function setOpen(open) {
        if (open) {
            sidebar.classList.remove('is-collapsed');
            main.classList.remove('is-expanded');
            if (window.innerWidth <= 992) overlay.classList.add('show');
        } else {
            sidebar.classList.add('is-collapsed');
            main.classList.add('is-expanded');
            overlay.classList.remove('show');
        }
        localStorage.setItem('admin_sidebar_open', open ? '1' : '0');
    }

    const saved = localStorage.getItem('admin_sidebar_open');
    const defaultOpen = window.innerWidth > 992;
    const startOpen = saved === null ? defaultOpen : (saved === '1');
    setOpen(startOpen);

    btnToggle.addEventListener('click', () => {
        const isCollapsed = sidebar.classList.contains('is-collapsed');
        setOpen(isCollapsed);
    });

    btnClose.addEventListener('click', () => setOpen(false));
    overlay.addEventListener('click', () => setOpen(false));

    window.addEventListener('resize', () => {
        if (window.innerWidth > 992) overlay.classList.remove('show');
    });
})();
