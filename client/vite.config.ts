import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react()],
    server: {
        port: 3000,      // 🔒 Forces Port 3000
        strictPort: true // 🛑 If 3000 is busy, it stops (doesn't jump to 5174)
    }
})
    