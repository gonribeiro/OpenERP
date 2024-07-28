/// <reference types="vite/client" />

interface ImportMetaEnv {
    readonly VITE_OPEN_ERP_API_URL: string;
    readonly VITE_HOME_PAGE: string;
}

interface ImportMeta {
    readonly env: ImportMetaEnv;
}