import React from "react";

interface SidebarProps {
    selected: string;
    onSelect: (section: string) => void;
}

const Sidebar: React.FC<SidebarProps> = ({ selected, onSelect }) => {
    const sections = ["Projects", "Employees", "Expenses", "Details","Users"];
    return (
        <div className="w-64 bg-gray-100 h-screen p-4">
            <h2 className="text-xl font-bold mb-4">Menu</h2>
            <ul>
                {sections.map((section) => (
                    <li
                        key={section}
                        onClick={() => onSelect(section)}
                        className={`p-2 cursor-pointer rounded ${selected === section ? "bg-blue-500 text-white" : "hover:bg-gray-300"}`}
                    >
                        {section}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default Sidebar;
