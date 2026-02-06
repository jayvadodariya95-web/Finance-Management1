//import { useEffect, useState } from "react";
//import type { ProjectDto, CreateProjectDto } from "../types";
//import { getProjects, createProject } from "../api/projectApi";
//import ProjectList from "../components/ProjectList";
//import AddProjectForm from "../components/AddProjectForm";

//const ProjectsPage = () => {
//    const [projects, setProjects] = useState<ProjectDto[]>([]);
//    const [showForm, setShowForm] = useState(false);

//    useEffect(() => {
//        const fetchProjects = async () => {
//            try {
//                const data = await getProjects();
//                setProjects(data);
//            } catch (error) {
//                console.error("Failed to load projects", error);
//            }
//        };

//        fetchProjects();
//    }, []);

//    const handleAddProject = async (project: CreateProjectDto) => {
//        try {
//            await createProject(project);
//            setShowForm(false);

//            // Reload projects after add
//            const data = await getProjects();
//            setProjects(data);
//        } catch (error) {
//            console.error("Failed to add project", error);
//        }
//    };

//    return (
//        <div>
//            <button onClick={() => setShowForm(true)}>
//                Add Project
//            </button>

//            {showForm && (
//                <AddProjectForm
//                    onSubmit={handleAddProject}
//                    onCancel={() => setShowForm(false)}
//                />
//            )}

//            <ProjectList projects={projects} />
//        </div>
//    );
//};

//export default ProjectsPage;


import { useEffect, useState } from "react";
import { Plus, Briefcase, DollarSign, Calendar, Users, X } from "lucide-react"; // Import icons
import type { ProjectDto, CreateProjectDto } from "../types";
import { getProjects, createProject } from "../api/projectApi";
import AddProjectForm from "../components/AddProjectForm";

const ProjectsPage = () => {
    const [projects, setProjects] = useState<ProjectDto[]>([]);
    const [showForm, setShowForm] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadProjects();
    }, []);

    const loadProjects = async () => {
        try {
            const data = await getProjects();
            setProjects(data);
        } catch (error) {
            console.error("Failed to load projects", error);
        } finally {
            setLoading(false);
        }
    };

    const handleAddProject = async (project: CreateProjectDto) => {
        try {
            await createProject(project);
            setShowForm(false);
            loadProjects(); // Reload list
        } catch (error) {
            console.error("Failed to add project", error);
        }
    };

    return (
        <div className="animate-fade-in">
            {/* --- Header Section --- */}
            <div className="flex justify-between items-center mb-8">
                <div>
                    <h1 className="text-3xl font-bold text-gray-900">Projects</h1>
                    <p className="text-gray-500 mt-1">Manage client projects and financial values</p>
                </div>
                <button
                    onClick={() => setShowForm(true)}
                    className="flex items-center gap-2 bg-blue-600 text-white px-5 py-2.5 rounded-lg hover:bg-blue-700 transition shadow-lg shadow-blue-200"
                >
                    <Plus size={20} /> New Project
                </button>
            </div>

            {/* --- Loading State --- */}
            {loading && (
                <div className="text-center py-20 text-gray-500">Loading projects...</div>
            )}

            {/* --- Project Grid (Rich UI) --- */}
            {!loading && (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {projects.map((p) => (
                        <div key={p.id} className="bg-white p-6 rounded-xl border border-gray-200 shadow-sm hover:shadow-md transition group">
                            <div className="flex justify-between items-start mb-4">
                                <div>
                                    <h3 className="font-bold text-lg text-gray-900 group-hover:text-blue-600 transition">{p.name}</h3>
                                    <p className="text-sm text-gray-500 flex items-center gap-1 mt-1">
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
                                    <span className="text-blue-600 truncate max-w-[120px]">{p.managedByPartner}</span>
                                </div>
                            </div>
                        </div>
                    ))}

                    {projects.length === 0 && !loading && (
                        <p className="col-span-full text-center text-gray-400 py-10">No projects found. Create one to get started.</p>
                    )}
                </div>
            )}

            {/* --- Modal Wrapper for Add Form --- */}
            {showForm && (
                <div className="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white w-full max-w-lg rounded-2xl shadow-2xl overflow-hidden relative">
                        {/* Close Button specific to Modal styling */}
                        <button
                            onClick={() => setShowForm(false)}
                            className="absolute top-4 right-4 text-gray-400 hover:text-red-500 transition z-10"
                        >
                            <X size={24} />
                        </button>

                        {/* Render the Form Component inside */}
                        <div className="p-1">
                            <AddProjectForm
                                onSubmit={handleAddProject}
                                onCancel={() => setShowForm(false)}
                            />
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ProjectsPage;
