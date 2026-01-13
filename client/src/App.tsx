import { useState, useEffect } from 'react';
import { useForm } from 'react-hook-form';
import {
    LayoutDashboard,
    FolderKanban,
    Users,
    Wallet,
    Plus,
    X,
    Calendar,
    DollarSign,
    Briefcase
} from 'lucide-react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { api } from './api/projectApi';
import type{ ProjectDto, CreateProjectDto } from './types';

export default function App() {
    const [activeTab, setActiveTab] = useState('projects');
    const [projects, setProjects] = useState<ProjectDto[]>([]);
    const [loading, setLoading] = useState(false);
    const [isModalOpen, setIsModalOpen] = useState(false);

    const { register, handleSubmit, reset } = useForm<CreateProjectDto>();

    useEffect(() => {
        if (activeTab === 'projects') {
            loadProjects();
        }
    }, [activeTab]);

    const loadProjects = async () => {
        setLoading(true);
        try {
            const data = await api.getProjects();
            setProjects(data);
        } catch (error) {
            console.log(error);
            toast.error("Failed to fetch projects. Is the API running?");
        } finally {
            setLoading(false);
        }
    };

    
    const onSubmit = async (data: CreateProjectDto) => {
        try {
           
            const payload = {
                ...data,
                projectValue: Number(data.projectValue),
                managedByPartnerId: Number(data.managedByPartnerId)
            };

            const newProject = await api.createProject(payload);

            toast.success("Project added successfully!");
            setProjects([...projects, newProject]); 
            setIsModalOpen(false);
            reset();
        } catch {
            toast.error("Error creating project. Check Partner ID.");
        }
    };

    return (
        <div className="flex h-screen bg-gray-50 text-gray-800 font-sans">

            {/* --- LEFT SIDEBAR --- */}
            <aside className="w-64 bg-slate-900 text-slate-300 flex flex-col fixed h-full shadow-xl">
                <div className="h-16 flex items-center justify-center text-xl font-bold text-white border-b border-slate-700 tracking-wider">
                    FINANCE<span className="text-blue-500">MGR</span>
                </div>

                <nav className="flex-1 p-4 space-y-2">
                    <SidebarItem
                        icon={<LayoutDashboard size={20} />}
                        label="Dashboard"
                        isActive={activeTab === 'dashboard'}
                        onClick={() => setActiveTab('dashboard')}
                    />
                    <SidebarItem
                        icon={<FolderKanban size={20} />}
                        label="Projects"
                        isActive={activeTab === 'projects'}
                        onClick={() => setActiveTab('projects')}
                    />
                    <SidebarItem
                        icon={<Users size={20} />}
                        label="Employees"
                        isActive={activeTab === 'employees'}
                        onClick={() => setActiveTab('employees')}
                    />
                    <SidebarItem
                        icon={<Wallet size={20} />}
                        label="Expenses"
                        isActive={activeTab === 'expenses'}
                        onClick={() => setActiveTab('expenses')}
                    />
                </nav>
            </aside>

            {/* --- RIGHT CONTENT AREA --- */}
            <main className="flex-1 ml-64 p-8 overflow-y-auto">

                {/* Only show this if "Projects" is selected */}
                {activeTab === 'projects' && (
                    <div className="max-w-7xl mx-auto animate-fade-in">
                        {/* Header */}
                        <div className="flex justify-between items-center mb-8">
                            <div>
                                <h1 className="text-3xl font-bold text-gray-900">Projects</h1>
                                <p className="text-gray-500 mt-1">Manage client projects and financial values</p>
                            </div>
                            <button
                                onClick={() => setIsModalOpen(true)}
                                className="flex items-center gap-2 bg-blue-600 text-white px-5 py-2.5 rounded-lg hover:bg-blue-700 transition shadow-lg shadow-blue-200"
                            >
                                <Plus size={20} /> New Project
                            </button>
                        </div>

                        {/* List / Grid */}
                        {loading ? (
                            <div className="text-center py-20 text-gray-500">Loading data from .NET API...</div>
                        ) : (
                            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                                {projects.map((p) => (
                                    <div key={p.id} className="bg-white p-6 rounded-xl border border-gray-200 shadow-sm hover:shadow-md transition group">
                                        <div className="flex justify-between items-start mb-4">
                                            <div>
                                                <h3 className="font-bold text-lg text-gray-900 group-hover:text-blue-600 transition">{p.name}</h3>
                                                <p className="text-sm text-gray-500 flex items-center gap-1">
                                                    <Briefcase size={12} /> {p.clientName}
                                                </p>
                                            </div>
                                            <span className={`px-2 py-1 rounded text-xs font-bold uppercase tracking-wide ${p.status === 'Active' ? 'bg-green-100 text-green-700' : 'bg-gray-100 text-gray-600'
                                                }`}>
                                                {p.status}
                                            </span>
                                        </div>

                                        <div className="space-y-3 text-sm text-gray-600 border-t border-gray-100 pt-4">
                                            <div className="flex items-center justify-between">
                                                <span className="flex items-center gap-2 text-gray-400"><DollarSign size={16} /> Value</span>
                                                <span className="font-semibold text-gray-900">${p.projectValue.toLocaleString()}</span>
                                            </div>
                                            <div className="flex items-center justify-between">
                                                <span className="flex items-center gap-2 text-gray-400"><Calendar size={16} /> Start</span>
                                                <span>{new Date(p.startDate).toLocaleDateString()}</span>
                                            </div>
                                            <div className="flex items-center justify-between">
                                                <span className="flex items-center gap-2 text-gray-400"><Users size={16} /> Manager</span>
                                                <span className="text-blue-600 truncate max-w-[120px]" title={p.managedByPartner}>{p.managedByPartner}</span>
                                            </div>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </div>
                )}

                {/* Placeholder for other tabs */}
                {activeTab !== 'projects' && (
                    <div className="flex items-center justify-center h-full text-gray-400 text-xl font-medium border-2 border-dashed border-gray-200 rounded-xl">
                        {activeTab.charAt(0).toUpperCase() + activeTab.slice(1)} Module Coming Soon
                    </div>
                )}
            </main>

            {/* --- ADD PROJECT MODAL --- */}
            {isModalOpen && (
                <div className="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white w-full max-w-lg rounded-2xl shadow-2xl overflow-hidden">
                        <div className="flex justify-between items-center p-6 border-b border-gray-100 bg-gray-50">
                            <h2 className="text-xl font-bold text-gray-800">Add New Project</h2>
                            <button onClick={() => setIsModalOpen(false)} className="text-gray-400 hover:text-red-500 transition">
                                <X size={24} />
                            </button>
                        </div>

                        <form onSubmit={handleSubmit(onSubmit)} className="p-6 space-y-4">
                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <label className="text-xs font-bold text-gray-500 uppercase">Name</label>
                                    <input {...register('name', { required: true })} className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Project Name" />
                                </div>
                                <div>
                                    <label className="text-xs font-bold text-gray-500 uppercase">Client</label>
                                    <input {...register('clientName', { required: true })} className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none" placeholder="Client Name" />
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <label className="text-xs font-bold text-gray-500 uppercase">Value ($)</label>
                                    <input type="number" {...register('projectValue', { required: true })} className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none" />
                                </div>
                                <div>
                                    <label className="text-xs font-bold text-gray-500 uppercase">Partner ID</label>
                                    <input type="number" {...register('managedByPartnerId', { required: true })} className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none" placeholder="e.g. 1" />
                                    <p className="text-[10px] text-gray-400 mt-1">Must exist in DB</p>
                                </div>
                            </div>

                            <div className="grid grid-cols-2 gap-4">
                                <div>
                                    <label className="text-xs font-bold text-gray-500 uppercase">Start Date</label>
                                    <input type="date" {...register('startDate', { required: true })} className="w-full border p-2 rounded-lg mt-1" />
                                </div>
                                <div>
                                    <label className="text-xs font-bold text-gray-500 uppercase">End Date</label>
                                    <input type="date" {...register('endDate')} className="w-full border p-2 rounded-lg mt-1" />
                                </div>
                            </div>

                            <div>
                                <label className="text-xs font-bold text-gray-500 uppercase">Description</label>
                                <textarea {...register('description')} className="w-full border p-2 rounded-lg mt-1 focus:ring-2 focus:ring-blue-500 outline-none" rows={3}></textarea>
                            </div>

                            <button type="submit" className="w-full bg-blue-600 text-white py-3 rounded-xl font-bold hover:bg-blue-700 transition shadow-lg">
                                Save Project
                            </button>
                        </form>
                    </div>
                </div>
            )}

            <ToastContainer position="bottom-right" />
        </div>
    );
}

// Helper Component for Sidebar Items
interface SidebarItemProps {
    icon: React.ReactNode;
    label: string;
    isActive: boolean;
    onClick: () => void;
}

function SidebarItem({ icon, label, isActive, onClick }: SidebarItemProps) {
    return (
        <div
            onClick={onClick}
            className={`flex items-center gap-3 px-4 py-3 rounded-lg cursor-pointer transition-all duration-200 ${isActive
                ? 'bg-blue-600 text-white shadow-lg shadow-blue-900/50'
                : 'text-slate-400 hover:bg-slate-800 hover:text-white'
                }`}
        >
            {icon}
            <span className="font-medium">{label}</span>
        </div>
    );
}