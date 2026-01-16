//import { useEffect, useState } from "react";
//import { Plus, Mail, Shield, X } from "lucide-react"; // Icons
//import type { UserDto, CreateUserDto } from "../types";
//import { getUsers, createUser } from "../api/userApi";
//import AddUserForm from "../components/AddUserForm";

//const UsersPage = () => {
//    const [users, setUsers] = useState<UserDto[]>([]);
//    const [showForm, setShowForm] = useState(false);
//    const [loading, setLoading] = useState(true);

//    useEffect(() => {
//        loadUsers();
//    }, []);

//    const loadUsers = async () => {
//        try {
//            const data = await getUsers();
//            setUsers(data);
//        } catch (error) {
//            console.error("Failed to load users", error);
//        } finally {
//            setLoading(false);
//        }
//    };

//    const handleAddUser = async (user: CreateUserDto) => {
//        try {
//            await createUser(user);
//            setShowForm(false);
//            loadUsers();
//        } catch (error) {
//            console.error("Failed to add user", error);
//        }
//    };

//    return (
//        <div className="animate-fade-in">
//            {/* --- Header --- */}
//            <div className="flex justify-between items-center mb-8">
//                <div>
//                    <h1 className="text-3xl font-bold text-gray-900">Users</h1>
//                    <p className="text-gray-500 mt-1">Manage system access and employees</p>
//                </div>
//                <button
//                    onClick={() => setShowForm(true)}
//                    className="flex items-center gap-2 bg-blue-600 text-white px-5 py-2.5 rounded-lg hover:bg-blue-700 transition shadow-lg shadow-blue-200"
//                >
//                    <Plus size={20} /> Add User
//                </button>
//            </div>

//            {loading && <div className="text-center py-20 text-gray-500">Loading users...</div>}

//            {/* --- User Cards --- */}
//            {!loading && (
//                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
//                    {users.map((user) => (
//                        <div key={user.id} className="bg-white p-6 rounded-xl border border-gray-200 shadow-sm hover:shadow-md transition flex flex-col items-center text-center">

//                            {/* Avatar Circle */}
//                            <div className="w-16 h-16 bg-blue-100 text-blue-600 rounded-full flex items-center justify-center mb-4 text-xl font-bold">
//                                {user.firstName[0]}{user.lastName[0]}
//                            </div>

//                            <h3 className="font-bold text-lg text-gray-900">{user.firstName} {user.lastName}</h3>

//                            {/* Role Badge */}
//                            <span className="mt-2 px-3 py-1 bg-purple-100 text-purple-700 text-xs font-bold uppercase rounded-full tracking-wider">
//                                {user.role}
//                            </span>

//                            <div className="w-full mt-6 space-y-3 pt-4 border-t border-gray-100">
//                                <div className="flex items-center justify-center gap-2 text-gray-500 text-sm">
//                                    <Mail size={16} />
//                                    <span>{user.email}</span>
//                                </div>
//                                <div className="flex items-center justify-center gap-2 text-gray-500 text-sm">
//                                    <Shield size={16} />
//                                    <span>System Access Granted</span>
//                                </div>
//                            </div>
//                        </div>
//                    ))}
//                </div>
//            )}

//            {/* --- Modal Wrapper --- */}
//            {showForm && (
//                <div className="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center z-50 p-4">
//                    <div className="bg-white w-full max-w-lg rounded-2xl shadow-2xl overflow-hidden relative">
//                        <button
//                            onClick={() => setShowForm(false)}
//                            className="absolute top-4 right-4 text-gray-400 hover:text-red-500 transition z-10"
//                        >
//                            <X size={24} />
//                        </button>
//                        <div className="p-1">
//                            <AddUserForm
//                                onSubmit={handleAddUser}
//                                onCancel={() => setShowForm(false)}
//                            />
//                        </div>
//                    </div>
//                </div>
//            )}
//        </div>
//    );
//};

//export default UsersPage;

import { useEffect, useState } from "react";
import { Plus, Mail, Shield, X } from "lucide-react";
import type { UserDto, CreateUserDto } from "../types";
import { getUsers, createUser } from "../api/userApi";
import AddUserForm from "../components/AddUserForm";

const UsersPage = () => {
    const [users, setUsers] = useState<UserDto[]>([]);
    const [showForm, setShowForm] = useState(false);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        loadUsers();
    }, []);

    const loadUsers = async () => {
        try {
            const data = await getUsers();
            setUsers(data);
        } catch (error) {
            console.error("Failed to load users", error);
        } finally {
            setLoading(false);
        }
    };

    const handleAddUser = async (user: CreateUserDto) => {
        try {
            await createUser(user);
            setShowForm(false);
            loadUsers(); // Refresh list to see new user with role
        } catch (error) {
            console.error("Failed to add user", error);
        }
    };

    // Helper to pick colors based on Role
    const getRoleBadgeStyle = (role: string) => {
        if (role === "Partner") return "bg-purple-100 text-purple-700 border-purple-200";
        if (role === "Employee") return "bg-blue-100 text-blue-700 border-blue-200";
        return "bg-gray-100 text-gray-600 border-gray-200"; // Fallback
    };

    return (
        <div className="animate-fade-in">
            {/* Header */}
            <div className="flex justify-between items-center mb-8">
                <div>
                    <h1 className="text-3xl font-bold text-gray-900">Users</h1>
                    <p className="text-gray-500 mt-1">Manage system access and employees</p>
                </div>
                <button
                    onClick={() => setShowForm(true)}
                    className="flex items-center gap-2 bg-blue-600 text-white px-5 py-2.5 rounded-lg hover:bg-blue-700 transition shadow-lg shadow-blue-200"
                >
                    <Plus size={20} /> Add User
                </button>
            </div>

            {loading && <div className="text-center py-20 text-gray-500">Loading users...</div>}

            {/* User Cards Grid */}
            {!loading && (
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                    {users.map((user) => (
                        <div key={user.id} className="bg-white p-6 rounded-xl border border-gray-200 shadow-sm hover:shadow-md transition flex flex-col items-center text-center">

                            {/* Avatar Circle */}
                            <div className="w-16 h-16 bg-blue-50 text-blue-600 border border-blue-100 rounded-full flex items-center justify-center mb-3 text-xl font-bold">
                                {user.firstName[0]}{user.lastName[0]}
                            </div>

                            {/* Name */}
                            <h3 className="font-bold text-lg text-gray-900 capitalize">
                                {user.firstName} {user.lastName}
                            </h3>

                            {/* 👇 ROLE BADGE IS HERE 👇 */}
                            <span className={`mt-2 px-3 py-1 text-xs font-extrabold uppercase rounded-full border tracking-wide ${getRoleBadgeStyle(user.role)}`}>
                                {user.role || "Unknown Role"}
                            </span>

                            {/* Info Section */}
                            <div className="w-full mt-6 space-y-3 pt-4 border-t border-gray-100">
                                <div className="flex items-center justify-center gap-2 text-gray-500 text-sm">
                                    <Mail size={16} />
                                    <span>{user.email}</span>
                                </div>
                                <div className="flex items-center justify-center gap-2 text-green-600 text-sm font-medium">
                                    <Shield size={16} />
                                    <span>System Access Granted</span>
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}

            {/* Add User Modal */}
            {showForm && (
                <div className="fixed inset-0 bg-black/60 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white w-full max-w-lg rounded-2xl shadow-2xl overflow-hidden relative">
                        <button
                            onClick={() => setShowForm(false)}
                            className="absolute top-4 right-4 text-gray-400 hover:text-red-500 transition z-10"
                        >
                            <X size={24} />
                        </button>
                        <AddUserForm
                            onSubmit={handleAddUser}
                            onCancel={() => setShowForm(false)}
                        />
                    </div>
                </div>
            )}
        </div>
    );
};

export default UsersPage;
