"use client";

import { useEffect, useState, use } from "react";
import { useLanguage } from "../../../context/LanguageContext";
import { apiFetch } from "../../../lib/api";
import { useRouter } from "next/navigation";
import { Ticket, ArrowLeft, ArrowRight, UserCheck, Clock } from "lucide-react";

interface TicketDetail {
  id: string;
  subject: string;
  description: string;
  status: string;
  priority: string;
  assigneeId?: string;
  createdAt?: string;
  tenantId?: string;
}

export default function TicketDetailPage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = use(params);
  const { t, dir } = useLanguage();
  const [ticket, setTicket] = useState<Ticket | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [newStatus, setNewStatus] = useState("");
  const [assigneeId, setAssigneeId] = useState("");
  const router = useRouter();

  useEffect(() => {
    apiFetch<Ticket>(`/tickets/${id}`)
      .then((data) => {
        setTicket(data);
        setNewStatus(data.status);
        setAssigneeId(data.assigneeId || "");
      })
      .catch((err) => {
        setError(err.message || "Failed to load ticket details");
      })
      .finally(() => setLoading(false));
  }, [id]);

  const handleStatusChange = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await apiFetch(`/tickets/${id}/status`, {
        method: "PUT",
        body: JSON.stringify({ status: newStatus }),
      });
      alert(t("success"));
    } catch (err: unknown) {
      alert((err as Error).message || "Failed to update status");
    }
  };

  const handleAssign = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await apiFetch(`/tickets/${id}/assign`, {
        method: "PUT",
        body: JSON.stringify({ assigneeId }),
      });
      alert(t("success"));
    } catch (err: unknown) {
      alert((err as Error).message || "Failed to assign ticket");
    }
  };

  if (loading) return <div className="text-center py-12 text-gray-500">{t("loading")}</div>;
  if (error || !ticket) return <div className="p-4 bg-red-50 text-red-700 rounded-lg">{error || "Ticket not found"}</div>;

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <div className="flex items-center space-x-3 rtl:space-x-reverse">
        <button
          onClick={() => router.back()}
          className="p-2 text-gray-600 hover:text-gray-900 rounded-lg hover:bg-gray-100 transition"
        >
          {dir === "rtl" ? <ArrowRight className="h-5 w-5" /> : <ArrowLeft className="h-5 w-5" />}
        </button>
        <div className="flex items-center space-x-2 rtl:space-x-reverse">
          <div className="p-2 bg-indigo-50 text-indigo-600 rounded-lg">
            <Ticket className="h-6 w-6" />
          </div>
          <h1 className="text-2xl font-bold text-gray-900">{t("ticketDetails")}</h1>
        </div>
      </div>

      <div className="bg-white shadow rounded-xl p-6 space-y-6 border border-gray-100">
        <div className="flex justify-between items-start">
          <div>
            <h2 className="text-xl font-bold text-gray-900">{ticket.subject}</h2>
            <p className="text-sm text-gray-500 mt-1 flex items-center">
              <Clock className="h-4 w-4 me-1 inline" />
              {ticket.createdAt ? new Date(ticket.createdAt).toLocaleString() : ""}
            </p>
          </div>
          <div className="flex space-x-2 rtl:space-x-reverse">
            <span className="px-3 py-1 text-xs font-semibold rounded-full bg-indigo-50 text-indigo-700">
              {ticket.status}
            </span>
            <span className="px-3 py-1 text-xs font-semibold rounded-full bg-amber-50 text-amber-700">
              {ticket.priority}
            </span>
          </div>
        </div>

        <div className="border-t border-gray-100 pt-4">
          <h3 className="text-sm font-medium text-gray-500 mb-2">{t("description")}</h3>
          <p className="text-gray-800 bg-gray-50 p-4 rounded-lg whitespace-pre-wrap">{ticket.description}</p>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-6 border-t border-gray-100 pt-6">
          <form onSubmit={handleStatusChange} className="space-y-3 bg-gray-50 p-4 rounded-xl">
            <h3 className="text-sm font-semibold text-gray-700">{t("changeStatus")}</h3>
            <select
              value={newStatus}
              onChange={(e) => setNewStatus(e.target.value)}
              className="w-full px-3 py-2 border border-gray-300 rounded-lg bg-white"
            >
              <option value="Open">{t("open")}</option>
              <option value="InProgress">{t("inProgress")}</option>
              <option value="Resolved">{t("resolved")}</option>
              <option value="Closed">{t("closed")}</option>
            </select>
            <button
              type="submit"
              className="w-full bg-indigo-600 text-white py-2 rounded-lg font-semibold hover:bg-indigo-700 transition"
            >
              {t("save")}
            </button>
          </form>

          <form onSubmit={handleAssign} className="space-y-3 bg-gray-50 p-4 rounded-xl">
            <h3 className="text-sm font-semibold text-gray-700">{t("assignee")}</h3>
            <input
              type="text"
              value={assigneeId}
              onChange={(e) => setAssigneeId(e.target.value)}
              placeholder="User ID / Assignee ID"
              className="w-full px-3 py-2 border border-gray-300 rounded-lg bg-white"
            />
            <button
              type="submit"
              className="w-full bg-indigo-600 text-white py-2 rounded-lg font-semibold hover:bg-indigo-700 transition flex items-center justify-center space-x-1 rtl:space-x-reverse"
            >
              <UserCheck className="h-4 w-4" />
              <span>{t("assign")}</span>
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
